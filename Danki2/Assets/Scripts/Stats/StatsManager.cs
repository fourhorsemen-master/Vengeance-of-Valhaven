using System;
using System.Collections.Generic;

public class StatsManager
{
    private readonly StatsDictionary _baseStats;
    private readonly Dictionary<Stat, int> _cache;
    private List<IStatPipe> _pipes;

    public StatsManager(StatsDictionary baseStats)
    {
        _baseStats = baseStats;
        _cache = new Dictionary<Stat, int>();
        _pipes = new List<IStatPipe>();
    }

    public int Get(Stat stat)
    {
        if (_cache.TryGetValue(stat, out int value))
        {
            return value;
        }
        else
        {
            float pipelineValue = _baseStats[stat];
            _pipes.ForEach(p => pipelineValue = p.ProcessStat(stat, pipelineValue));

            int statValue = (int)Math.Ceiling(pipelineValue);
            _cache.Add(stat, statValue);
            return statValue;
        }
    }

    public void RegisterPipe(IStatPipe pipe)
    {
        _pipes.Add(pipe);
    }

    public void ClearCache()
    {
        _cache.Clear();
    }
}
