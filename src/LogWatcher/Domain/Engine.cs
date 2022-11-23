﻿using Domain.Database;
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

        public Engine()
        {
            sqlConnect = new SqlConnect();
            fileLoader = new FileLoader();
        }


        public async Task AddSourceSystem(SourceSystem sourceSystem)
        {
            // insert new system into db
            var newRecord = await sqlConnect.CreateSourceSystem(sourceSystem);            
            await UpdateFilesFromSourceSystem(newRecord);
        }

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
                await AddLogFileToDB(sourceSystem, file);
            });
            await Task.WhenAll(addFilesTasks);            
        }

        // foreach line in file
        // create and add line to LogLine db
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

        private async Task<List<string>> GetFilesInDirectory(string sourcePath)
        {
            var results = new List<string>();
            var directoryExists = await Task.Run(() => { return Directory.Exists(sourcePath); });
            if (!directoryExists) return results;

            var fileTask = await Task.Run(() => { return Directory.GetFiles(sourcePath); });
            results = fileTask.ToList();            

            return results;
        }

        private async Task<List<LogFile>> GetFilesInDB(SourceSystem sourceSystemD)
        {
            var results = await sqlConnect.GetAllLogFiles(sourceSystemD);
            return results;
        }

        private async Task<List<LogLine>> GetLinesFromFile(SourceSystem source, string fileName, int logFileId)
        {
            var fullPath = Path.Combine(source.SourceFolder, fileName);
            var allLogLines = await fileLoader.GetLines(fullPath, source, logFileId);

            return allLogLines;
        }
    }
}
