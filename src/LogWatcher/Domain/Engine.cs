﻿using Domain.API;
using Domain.Database;
using Domain.FileSystem;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Engine
    {
        private SqlConnect sqlConnect;
        private FileLoader fileLoader;
        private Connector connector;

        public Engine()
        {
            sqlConnect = new SqlConnect();
            fileLoader = new FileLoader();
            connector = new Connector();
        }

        #region public methods

        /// <summary>
        /// Retrieve all SourceSystems from DB
        /// </summary>
        public async Task<List<SourceSystem>> GetAllSourceSystems()
        {
            var ssList = new List<SourceSystem>();
            ssList = await sqlConnect.GetAllSourceSystems();
            return ssList;
        }

        /// <summary>
        /// Add new SourceSystem to database
        /// </summary>
        public async Task<SourceSystem> AddSourceSystem(SourceSystem sourceSystem)
        {
            // insert new system into db
            var newRecord = await sqlConnect.CreateSourceSystem(sourceSystem);            
            await UpdateFilesFromSourceSystem(newRecord);

            return newRecord;
        }

        /// <summary>
        /// Remove SourceSystem and associated logfiles and lines.
        /// </summary>
        public async Task RemoveSourceSystem(SourceSystem sourceSystem)
        {
            // first get loglines to remove from DB
            var linesToDelete = await GetFilesInDB(sourceSystem);

            // then remove logfiles and lines
            await DeleteLogFilesAndLinesFromDB(linesToDelete);

            // then remove sourceSystem
            await sqlConnect.DeleteSourceSystem(sourceSystem);
        }

        /// <summary>
        /// Update logfiles and -lines for SourceSystem
        /// </summary>
        public async Task UpdateSourceSystem(SourceSystem sourceSystem)
        {
            var updated = await sqlConnect.UpdateSourceSystem(sourceSystem);

            await UpdateFilesFromSourceSystem(updated);
        }

        /// <summary>
        /// Refresh logfiles and -lines for SourceSystem
        /// </summary>
        public async Task UpdateFilesFromSourceSystem(SourceSystem sourceSystem)
        {
            // get list of all files in sourceDir
            var filesInDir = await GetFilesInDirectory(sourceSystem.SourceFolder);

            // get list of existing LogFiles from DB for sourceSystem
            var filesInDb = await GetFilesInDB(sourceSystem);

            // for each file in sourceDir
            // if file exists in db -> remove all entries for logfile from db, remove logfile from db

            // get list of files already loaded in db
            var overlapFiles = filesInDb.Select(db => db.FileName).Intersect(filesInDir.Select(dir => Path.GetFileName(dir))).ToList();
            var filesToPurgeFromDB = filesInDb.Where(file => overlapFiles.Contains(file.FileName))?.ToList();
            
            // delete pre-existing logfiles + loglines from db
            await DeleteLogFilesAndLinesFromDB(filesToPurgeFromDB);

            // add file to LogFiles db
            var newLogFiles = new List<LogFile>();
            var addFilesTasks = filesInDir.Select(async file =>
            {
                await AddLogFileToDB(sourceSystem, Path.GetFileName(file));
            });
            await Task.WhenAll(addFilesTasks);            
        }

        public async Task SendQueryToParser(SearchSet searchSet)
        {            
            // TODO: should we await response as confirmation of query receieved ?
            await connector.SendSearch(searchSet);
        }

        public async Task<string> RetrieveResultsFromParser()
        {
            string results = null;

            while (results == null)
            {
                await Task.Delay(1000);
                results = await connector.PullResult();                
            }

            var parsedResults = ParseStringToHitList(results);
            return "";
        }
        #endregion

        #region private methods
        
        // database interaction
        private async Task<List<LogFile>> GetFilesInDB(SourceSystem sourceSystem)
        {
            var results = await sqlConnect.GetAllLogFiles(sourceSystem);
            return results;
        }
        private async Task AddLogFileToDB(SourceSystem sourceSystem, string fileName)
        {  
            var logFile = new LogFile { FileName= fileName, SourceSystemID = sourceSystem.ID, SourceSystemName = sourceSystem.Name, FileHash = "" };

            var logFileId = await sqlConnect.CreateLogFile(logFile);
            if (logFileId > 0) 
            {
                var logLines = await GetLinesFromFile(sourceSystem, fileName, logFileId);
                await sqlConnect.CreateLogLines(logLines);
            }
        }

        private async Task DeleteLogFilesAndLinesFromDB(List<LogFile> filesToPurgeFromDB)
        {
            var deleteTasks = filesToPurgeFromDB.Select(async file =>
            {
                await sqlConnect.DeleteLogLinesForLogFile(file);
                await sqlConnect.DeleteLogFile(file);
            });
            await Task.WhenAll(deleteTasks);
        }

        // filesystem interaction
        private async Task<List<string>> GetFilesInDirectory(string sourcePath)
        {
            var results = new List<string>();
            var directoryExists = await Task.Run(() => { return Directory.Exists(sourcePath); });
            if (!directoryExists) return results;

            var fileTask = await Task.Run(() => { return Directory.GetFiles(sourcePath); });
            results = fileTask.ToList();            

            return results;
        }

        private async Task<List<LogLine>> GetLinesFromFile(SourceSystem source, string fileName, int logFileId)
        {
            var fullPath = Path.Combine(source.SourceFolder, fileName);
            var allLogLines = await fileLoader.GetLines(fullPath, source, logFileId);

            return allLogLines;
        }
        #endregion

        private HitList ParseStringToHitList(string parserResults)
        {
            //parserResults = "[[4,8,272],[4,8,240],[4,8,296],[4,8,279],[4,7,272],[4,7,240],[4,7,279],[4,7,296]]";

            var _trimmed = parserResults.Substring(2, parserResults.Length - 4).Split("],[").ToList();
            var hitList = new HitList();

            _trimmed.ForEach(hit =>
                {
                    var hitIds = hit.Split(',').ToList().Select(hit => Int32.Parse(hit.Trim())).ToArray();
                    hitList.AddHit(hitIds[0], hitIds[1], hitIds[2]);
                });

            return hitList;
        }
    }
}
