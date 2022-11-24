using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class SearchSet
    {
        public List<SourceSystem> SourceSystems { get; set; } = new List<SourceSystem>();

        public string KeyWordList { get; set; }

        public (DateTime Start, DateTime End) SearchPeriod { get; set; }
    }
}
