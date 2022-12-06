using Domain.API;
using Domain.Database;
using Domain.FileSystem;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Domain
{
    public class Engine
    {
        private SqlConnect sqlConnect;
        private FileLoader fileLoader;
        private Connector connector;

        public SearchSet OldSearch { get; private set; }

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

        /// <summary>
        /// Send HTTP query to LogParser API.
        /// </summary>
        public async Task SendQueryToParser(SearchSet searchSet)
        {
            var endpoint = IsQueryReSearch(searchSet) ? "research" : "search";
            await connector.SendSearch(searchSet, endpoint);
        }

        /// <summary>
        /// Query LogParser API for result.
        /// </summary>
        public async Task<List<LogLine>> RetrieveResultsFromParser()
        {
            string results = null;
            while (results == null)
            {
                await Task.Delay(10);
                results = await connector.PullResult();                
            }

            var parsedResults = ParseStringToHitList(results);
            var resultList = new List<LogLine>();

            foreach (var systemID in parsedResults.LogHits.Keys)
            {
                foreach (var fileID in parsedResults.LogHits[systemID])
                {
                    foreach (var lineID in fileID.Value)
                    {
                        var line = await sqlConnect.GetLogLineById(lineID);
                        resultList.Add(line);
                    }
                }
            }

            return resultList;
        }

        /// <summary>
        /// Get result directly from database (when user doesn't provide keywords in search)
        /// </summary>
        /// <param name="searchSet"></param>
        /// <returns></returns>
        public async Task<List<LogLine>> RetrieveResultsFromDatabase(SearchSet searchSet)
        {
            var resultList = new List<LogLine>();

            foreach (var ss in searchSet.SourceSystems)
            {
                var files = await sqlConnect.GetAllLogFiles(ss);
                foreach (var file in files)
                {
                    var logLines = await sqlConnect.GetAllLogLinesBySSIDandLogFileIdAndPeriod(ss.ID, file.ID, searchSet);
                    resultList.AddRange(logLines);
                }
            }

            return resultList;
        }

        public async Task OpenLogFile(LogLine selectedLine)
        {
            var sourceSystem = await sqlConnect.GetSourceSystemById(selectedLine.SourceSystemID);
            if (sourceSystem == null) return;

            var sourceFile = await sqlConnect.GetLogFileById(selectedLine.LogFileID);
            if (sourceFile == null) return;

            var fileDirectory = sourceSystem.SourceFolder;
            var fileName = sourceFile.FileName;
            var completedPath = Path.Combine(sourceSystem.SourceFolder, sourceFile.FileName);

            var fileExists = await Task.Run(() => { return File.Exists(completedPath); });
            if (fileExists)
            {
                await FileSystem.OpenLogFile.OpenWithDefault(completedPath);
            }
        }

        /// <summary>
        /// Used for ensuring we never /research if files are updated
        /// </summary>
        public void ResetOldSearch()
        {
            OldSearch = null;
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

        private HitCollection ParseStringToHitList(string parserResults)
        {
            var hitList = new HitCollection();
            if (string.IsNullOrEmpty(parserResults) || parserResults == "null") return hitList;

            var _trimmed = parserResults.Substring(2, parserResults.Length - 4).Split("],[").ToList();
            _trimmed.ForEach(hit =>
                {
                    var hitIds = hit.Split(',').ToList().Select(hit => Int32.Parse(hit.Trim())).ToArray();
                    hitList.AddHit(hitIds[0], hitIds[1], hitIds[2]);
                });

            return hitList;
        }

        private bool IsQueryReSearch(SearchSet newSearch) 
        {
            if (OldSearch != null)
            {
                var ssNew = string.Join(",", newSearch.SourceSystems.Select(s => s.ID));
                var ssOld = string.Join(",", OldSearch.SourceSystems.Select(s => s.ID));
                if (ssNew != ssOld)
                {
                    return false;
                }

                if (newSearch.SearchPeriod.Start == OldSearch.SearchPeriod.Start && newSearch.SearchPeriod.End == OldSearch.SearchPeriod.End) 
                {
                    return true;
                }
            }
            OldSearch = newSearch;
            return false;
        }
        #endregion
    }
}