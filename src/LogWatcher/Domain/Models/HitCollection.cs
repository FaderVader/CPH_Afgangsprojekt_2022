using System.Collections.Generic;

namespace Domain.Models
{
    public class HitCollection
    {
        public Dictionary<int, Dictionary<int, List<int>>> Hits = new Dictionary<int, Dictionary<int, List<int>>>();

        public void AddHit(int system, int file, int line)
        {
            if (!Hits.ContainsKey(system))
            {
                var _file = new Dictionary<int, List<int>>() { { file, new List<int> { line } } };
                Hits.Add(system, _file);
            } else
            {
                if (!Hits[system].ContainsKey(file))
                {
                    Hits[system].Add(file, new List<int>() { line });
                } else
                {
                    Hits[system][file].Add(line);
                }
            }
        }
    }
}