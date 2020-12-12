using System.Collections.Generic;
using UnityEngine;

public class StatsManager
{
    private readonly StatsDictionary baseStats;
    private readonly Dictionary<Stat, int> cache = new Dictionary<Stat, int>();
    private readonly List<IStatPipe> pipes = new List<IStatPipe>();

    public StatsManager(StatsDictionary baseStats)
    {
        this.baseStats = baseStats;
    }

    public int Get(Stat stat)
    {
        if (cache.TryGetValue(stat, out int value)) return value;

        float pipelineValue = baseStats[stat];
        pipes.ForEach(p => pipelineValue = p.ProcessStat(stat, pipelineValue));

        int statValue = Mathf.CeilToInt(pipelineValue);
        cache.Add(stat, statValue);
        return statValue;
    }

    public void RegisterPipe(IStatPipe pipe)
    {
        pipes.Add(pipe);
        ClearCache();
    }

    public void ClearCache() => cache.Clear();
}
