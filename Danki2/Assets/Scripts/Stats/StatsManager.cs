using System;

public class StatsManager
{
    private readonly Stats _baseStats;
    private readonly Stats _frameStats;

    public StatsManager(Stats baseStats)
    {
        _baseStats = baseStats;
        _frameStats = new Stats(_baseStats);
    }

    public int this[Stat stat]
    {
        get { return _frameStats[stat]; }
        set { _frameStats[stat] = value; }
    }

    public void Rebase()
    {
        foreach (Stat stat in Enum.GetValues(typeof(Stat)))
        {
            _frameStats[stat] = _baseStats[stat];
        }
    }
}
