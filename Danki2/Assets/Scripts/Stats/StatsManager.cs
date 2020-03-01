using System;
using System.Collections.Generic;

public class StatsManager
{
    private readonly StatsDictionary _baseStats;
    private readonly Dictionary<Stat, int> _cache;
    private List<StatPipe> _pipes;

    public StatsManager(StatsDictionary baseStats)
    {
        _baseStats = baseStats;
        _cache = new Dictionary<Stat, int>();
        _pipes = new List<StatPipe>();
    }

    public int this[Stat stat]
    {
        get {
            if (_cache.TryGetValue(stat, out int value))
            {
                return value;
            }
            else
            {
                var pipelineValue = (float)_baseStats[stat];
                _pipes.ForEach(p => pipelineValue = p.ProcessStat(stat, pipelineValue));

                var statValue = (int)Math.Ceiling(pipelineValue);
                _cache.Add(stat, statValue);
                return statValue;
            }
        }
    }

    public void RegisterPipe(StatPipe pipe)
    {
        _pipes.Add(pipe);
    }

    public void ClearCache()
    {
        _cache.Clear();
    }
}
