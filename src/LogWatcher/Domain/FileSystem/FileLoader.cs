using Domain.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.FileSystem
{
    public class FileLoader
    {
        public async Task<List<LogLine>> GetLines(string filePath, SourceSystem source, int logFileId)
        {
            var lines = await File.ReadAllLinesAsync(filePath);
            var logLines = new List<LogLine>();

            lines.ToList().ForEach(line =>
            {
                var timeStamp = line.Substring(0, 33);
                var severity = line.Substring(34, 6);
                var description = line.Substring(40);
                var module = "";

                var isValidLine = DateTime.TryParse(timeStamp, out _);

                if (isValidLine)
                {
                    var logLine = new LogLine
                    {
                        SourceSystemID = source.ID,
                        LogFileID = logFileId,
                        TimeOfEvent = DateTime.Parse(timeStamp),
                        Severity = severity,
                        EventDescription = description,
                        SourceModule = module,
                        Rawtext = line
                    };

                    logLines.Add(logLine);
                }
            });            

            return logLines;
        }
    }
}
