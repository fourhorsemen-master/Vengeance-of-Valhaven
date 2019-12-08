using System;
using System.Collections.Generic;

public class StatsManager
{
    private readonly Stats _baseStats;
    private readonly Dictionary<Stat, float> _statsCache;
    private List<Func<Stat, float, float>> _pipes;

    public StatsManager(Stats baseStats)
    {
        _baseStats = baseStats;
        _statsCache = new Dictionary<Stat, float>();
        _pipes = new List<Func<Stat, float, float>>();
    }

    public float this[Stat stat]
    {
        get {
            if (_statsCache.TryGetValue(stat, out float value))
            {
                return value;
            }
            else
            {
                var pipelineValue = (float)_baseStats[stat];
                _pipes.ForEach(p => pipelineValue = p(stat, pipelineValue));
                _statsCache.Add(stat, pipelineValue);
                return pipelineValue;
            }
        }
    }

    public void RegisterPipe(Func<Stat, float, float> pipe)
    {
        _pipes.Add(pipe);
    }

    public void ResetCache()
    {
        _statsCache.Clear();
    }
}
