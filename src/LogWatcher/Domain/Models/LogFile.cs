using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models
{
    public class LogFile
    {
        public int ID { get; init; }

        public int SourceSystemID { get; set; }

        public string SourceSystemName { get; set; }

        public string FileName { get; set; }

        public string FileHash { get; set; }

        public List<LogLine> LogLines { get; set; }
    }
}
