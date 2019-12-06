using System;
using UnityEngine;

public class StatsManager : MonoBehaviour
{
    [HideInInspector]
    public Stats baseStats = new Stats(0);

    private Stats _baseStats;
    private Stats _frameStats;

    // We initialse Stats fields in the constructor because errors happen if we wait until Awake or Start - not clear why.
    public StatsManager()
    {
        _baseStats = baseStats;
        _frameStats = new Stats(_baseStats);
    }

    public void Rebase()
    {
        foreach (Stat stat in Enum.GetValues(typeof(Stat)))
        {
            _frameStats[stat] = _baseStats[stat];
        }
    }

    public int this[Stat stat]
    {
        get { return _frameStats[stat]; }
        set { _frameStats[stat] = value; }
    }
}
