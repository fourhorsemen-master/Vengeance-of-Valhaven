using System.Collections.Generic;

public class StatsManager
{
    private readonly Stats _baseStats;
    private readonly Dictionary<Stat, float> _cache;
    private List<StatPipe> _pipes;

    public StatsManager(Stats baseStats)
    {
        _baseStats = baseStats;
        _cache = new Dictionary<Stat, float>();
        _pipes = new List<StatPipe>();
    }

    public float this[Stat stat]
    {
        get {
            if (_cache.TryGetValue(stat, out float value))
            {
                return value;
            }
            else
            {
                var pipelineValue = (float)_baseStats[stat];
                _pipes.ForEach(p => pipelineValue = p.ProcessStat(stat, pipelineValue));
                _cache.Add(stat, pipelineValue);
                return pipelineValue;
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
