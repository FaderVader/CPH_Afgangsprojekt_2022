using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models
{
    public class SourceSystem
    {
        public int ID { get; init; }

        public string Name { get; set; }

        public string SourceFolder { get; set; }

        public string LineTemplate { get; set; }
    }
}
