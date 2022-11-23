using System;
using Dapper;
using System.Data;
using System.Data.SqlClient;
using Domain.Models;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections;

namespace Domain.Database
{
    public class SqlConnect
    {
        private const string connString = "Server=localhost;Database=LogStore;UID=sa;PWD=Jakob12345!;";

        public async Task<SourceSystem> CreateSourceSystem(SourceSystem sourceSystem)
        {
            var query = $"INSERT INTO SourceSystems (Name, SourceFolder, LineTemplate) VALUES (@Name, @SourceFolder, @LineTemplate)";
            var parameters = new DynamicParameters();
            parameters.Add("Name", sourceSystem.Name, DbType.String);
            parameters.Add("SourceFolder", sourceSystem.SourceFolder, DbType.String);
            parameters.Add("LineTemplate", sourceSystem.LineTemplate, DbType.String);

            using (IDbConnection connection = new SqlConnection(connString))
            {
                var createResult = await connection.ExecuteAsync(query, parameters);

                if (createResult > 0)
                {
                    query = $"SELECT * FROM SourceSystems WHERE Name = '{sourceSystem.Name}' AND SourceFolder = '{sourceSystem.SourceFolder}' AND LineTemplate = '{sourceSystem.LineTemplate}'";
                    var newRecordResult = await connection.QueryAsync<SourceSystem>(query);
                    var result = newRecordResult.FirstOrDefault();

                    if (result != null) 
                    {
                        var newRecord = new SourceSystem
                        {
                            ID = result.ID,
                            Name = result.Name,
                            SourceFolder = result.SourceFolder,
                            LineTemplate = result.LineTemplate,
                        };

                        return newRecord;
                    }
                }
                return sourceSystem;
            };
        }

        public async Task<List<SourceSystem>> GetAllSourceSystems()
        {
            var query = "SELECT * FROM SourceSystems";

            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(connString))
            {
                var result = await connection.QueryAsync<SourceSystem>(query);
                return result.ToList(); ;
            };

        }

        /// <summary>
        /// Get list of available LogFiles from DB based on SourceSystemID.
        /// </summary>
        /// <param name="sourceSystem"></param>
        /// <returns></returns>
        public async Task<List<LogFile>> GetAllLogFiles(SourceSystem sourceSystem)
        {
            var sourceID = sourceSystem.ID;

            using (IDbConnection connection = new SqlConnection(connString))
            {
                var results = await connection.QueryAsync<LogFile>($"SELECT * FROM LogFiles WHERE SourceSystemID = {sourceID}");
                return results.ToList();
            }
        }

        public async Task<LogFile> GetLogFileById(int id)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(connString))
            {
                var result = await connection.QueryAsync<LogFile>($"SELECT * FROM LogFiles WHERE Id = {id}");
                return result.FirstOrDefault();
            };
        }

        /// <summary>
        /// Will return record-id if succesfull
        /// </summary>
        /// <param name="logFile"></param>
        /// <returns></returns>
        public async Task<int> CreateLogFile(LogFile logFile)
        {
            var query = "INSERT INTO LogFiles (SourceSystemID, FileName, FileHash) VALUES (@SourceSystemID, @FileName, @FileHash)";
            var parameters = new DynamicParameters();
            parameters.Add("SourceSystemID", logFile.SourceSystemID, DbType.Int32);
            parameters.Add("FileName", logFile.FileName, DbType.String);
            parameters.Add("FileHash", logFile.FileHash, DbType.String);


            using (IDbConnection connection = new SqlConnection(connString))
            {
                var result = await connection.ExecuteAsync(query, parameters);

                if (result > 0)
                {
                    query = $"SELECT * FROM LogFiles WHERE " +
                        $"SourceSystemId = {logFile.SourceSystemID} AND " +
                        $"FileName = '{logFile.FileName}' AND " +
                        $"FileHash = '{logFile.FileHash}';";

                    var newRecordResult = await connection.QueryAsync<LogFile>(query) ;

                    if (newRecordResult.FirstOrDefault() != null ) return newRecordResult.FirstOrDefault().ID;
                }
                return 0;
            };
        }

        public async Task DeleteLogFile(LogFile logFile)
        {
            var query = $"DELETE FROM LogFiles WHERE SourceSystemID = {logFile.SourceSystemID} AND FileName = '{logFile.FileName}';";
            using (IDbConnection connection = new SqlConnection(connString))
            {
                await connection.ExecuteAsync(query);
            };
        }

        public async Task DeleteLogLinesForLogFile(LogFile logFile)
        {
            var query = $"DELETE FROM LogLines WHERE SourceSystemID = {logFile.SourceSystemID} AND LogFileID = {logFile.ID}";
            using (IDbConnection connection = new SqlConnection(connString))
            {
                await connection.ExecuteAsync(query);
            };
        }

        public async Task<int> CreateLogLines(List<LogLine> logLines)
        {
            var query = "INSERT INTO LogLines (SourceSystemID, LogFileId, TimeOfEvent, Severity, EventDescription, SourceModule, RawText) " +
                "VALUES (@SourceSystemID, @LogFileId, @TimeOfEvent, @Severity, @EventDescription, @SourceModule, @RawText)";

            var lineCounter = 0;

            try
            {
                using (IDbConnection connection = new SqlConnection(connString))
                {
                    /*var createTasks = logLines.Select(async line =>
                    {
                        var parameters = new DynamicParameters();
                        parameters.Add("SourceSystemID", line.SourceSystemID, DbType.Int32);
                        parameters.Add("LogFileId", line.LogFileID, DbType.Int32);
                        parameters.Add("TimeOfEvent", line.TimeOfEvent, DbType.DateTime);
                        parameters.Add("Severity", line.Severity, DbType.String);
                        parameters.Add("EventDescription", line.EventDescription, DbType.String);
                        parameters.Add("SourceModule", line.SourceModule, DbType.String);
                        parameters.Add("RawText", line.Rawtext, DbType.String);

                        var results = await connection.ExecuteAsync(query, parameters);

                        lineCounter++;

                    });
                    await Task.WhenAll(createTasks);*/

                    foreach (var line in logLines)
                    {
                        var parameters = new DynamicParameters();
                        parameters.Add("SourceSystemID", line.SourceSystemID, DbType.Int32);
                        parameters.Add("LogFileId", line.LogFileID, DbType.Int32);
                        parameters.Add("TimeOfEvent", line.TimeOfEvent, DbType.DateTime);
                        parameters.Add("Severity", line.Severity, DbType.String);

                        var descript = line.EventDescription.Substring(0, Math.Min(line.EventDescription.Length, 1020));
                        parameters.Add("EventDescription", descript, DbType.String);

                        var source = line.SourceModule.Substring(0, Math.Min(line.SourceModule.Length, 1020));
                        parameters.Add("SourceModule", source, DbType.String);

                        var raw = line.Rawtext.Substring(0, Math.Min(line.Rawtext.Length, 2040));
                        parameters.Add("RawText", raw, DbType.String);

                        

                        await connection.ExecuteAsync(query, parameters);
                        lineCounter++;
                    }

                    return lineCounter;
                }
            }
            catch (Exception e)
            {

                throw;
            }
        }
    }
}
