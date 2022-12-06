using System.Collections.Generic;

namespace Domain.Models
{
    public class HitCollection
    {
        public Dictionary<int, Dictionary<int, List<int>>> LogHits { get; set; } = new Dictionary<int, Dictionary<int, List<int>>>();

        public void AddHit(int system, int file, int line)
        {
            if (!LogHits.ContainsKey(system))
            {
                var _file = new Dictionary<int, List<int>>() { { file, new List<int> { line } } };
                LogHits.Add(system, _file);
            } else
            {
                if (!LogHits[system].ContainsKey(file))
                {
                    LogHits[system].Add(file, new List<int>() { line });
                } else
                {
                    LogHits[system][file].Add(line);
                }
            }
        }
    }
}