using System;

namespace Domain.Models
{
    public class LogLine
    {
        public int ID { get; init; }
        public int SourceSystemID { get; set; }
        public int LogFileID { get; set; }
        public DateTime TimeOfEvent { get; set; }
        public string Severity { get; set; }
        public string EventDescription { get; set; }
        public string SourceModule { get; set; }
        public string Rawtext { get; set; }
    }
}