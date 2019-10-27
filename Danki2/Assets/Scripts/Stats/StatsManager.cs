using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsManager
{
    private readonly Stats baseStats;
    private readonly Stats frameStats;

    public StatsManager(Stats baseStats)
    {
        this.baseStats = baseStats;
        this.frameStats = new Stats(this.baseStats);
    }

    public int this[Stat stat]
    {
        get { return frameStats[stat]; }
        set { frameStats[stat] = value; }
    }

    public void Rebase()
    {
        foreach (Stat stat in Enum.GetValues(typeof(Stat)))
        {
            frameStats[stat] = baseStats[stat];
        }
    }
}
