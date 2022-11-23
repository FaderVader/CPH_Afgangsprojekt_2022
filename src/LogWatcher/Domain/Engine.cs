using Domain.Database;
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

        public Engine()
        {
            sqlConnect = new SqlConnect();
        }


        public async Task AddSourceSystem(SourceSystem sourceSystem)
        {
            // insert new system into db
        }

        public async Task UpdateFilesFromSourceSystem(SourceSystem sourceSystem)
        {
            // get list of all files in sourceDir
            var filesInDir = await GetFilesInDirectory(sourceSystem.SourceFolder);

            // get list of existing LogFiles from DB for sourceSystem
            var filesInDb = await GetFilesInDB(sourceSystem);

            // get list of files already loaded in db
            var overlapFiles = filesInDb.Select(db => db.FileName).Intersect(filesInDir.Select(dir => Path.GetFileName(dir))).ToList();
            var filesToPurgeFromDB = filesInDb.Where(file => overlapFiles.Contains(file.FileName))?.ToList();
            
            await DeleteLogFilesFromDB(filesToPurgeFromDB);


            // for each file in sourceDir
            // if file exists in db -> remove all entries for logfile from db, remove logfile from db
            // else
            // add file to LogFiles db
            // foreach line in file
            // create and add line to LogLine db
        }

        private async Task DeleteLogFilesFromDB(List<LogFile> filesToPurgeFromDB)
        {
            var deleteTasks = filesToPurgeFromDB.Select(async file =>
            {
                await sqlConnect.DeleteLogLinesForLogFile(file);
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
    }
}
