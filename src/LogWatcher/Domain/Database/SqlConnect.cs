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

        /// <summary>
        /// Get list of available LogFiles from DB based on SourceSystemID.
        /// </summary>
        /// <param name="sourceSystem"></param>
        /// <returns></returns>
        public async Task<List<LogFile>> GetAllLogFiles(SourceSystem sourceSystem)
        {
            var sourceID = sourceSystem.ID;

            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(connString))
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

        public async Task CreateLogFile(LogFile logFile)
        {
            // "INSERT LogFiles(SourceSystemID, FileName, FileHash)\r\nVALUES (11, 'test2.log', 'bbb');"
            var query = "INSERT INTO LogFiles (SourceSystemID, FileName, FileHash) VALUES (@SourceSystemID, @FileName, @FileHash)";
            var parameters = new DynamicParameters();
            parameters.Add("SourceSystemID", logFile.SourceSystemID, DbType.Int32);
            parameters.Add("FileName", logFile.FileName, DbType.String);
            parameters.Add("FileHash", logFile.FileHash, DbType.String);


            using (IDbConnection connection = new SqlConnection(connString))
            {
                await connection.ExecuteAsync(query, parameters);
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
    }
}
