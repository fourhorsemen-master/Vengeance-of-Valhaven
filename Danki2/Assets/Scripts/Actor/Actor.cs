using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    [HideInInspector]
    public Stats baseStats = new Stats(0);

    private StatsManager statsManager;

    // Start is called before the first frame update
    void Start()
    {
        statsManager = new StatsManager(baseStats);
    }

    // Update is called once per frame
    void Update()
    {
        statsManager.Rebase();
    }

    public int GetStat(Stat stat)
    {
        return this.statsManager[stat];
    }

    public void SetStat(Stat stat, int value)
    {
        this.statsManager[stat] = value;
    }
}
