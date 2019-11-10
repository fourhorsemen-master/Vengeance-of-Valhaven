using Assets.Scripts.Effects;
using UnityEngine;

public class Actor : MonoBehaviour
{
    [HideInInspector]
    public Stats baseStats = new Stats(0);

    private StatsManager statsManager;

    private EffectTracker effectTracker;

    // Start is called before the first frame update
    void Start()
    {
        statsManager = new StatsManager(baseStats);
        effectTracker = new EffectTracker(this);
    }

    // Update is called once per frame
    void Update()
    {
        statsManager.Rebase();
        effectTracker.ProcessEffects();
    }

    public int GetStat(Stat stat)
    {
        return statsManager[stat];
    }

    public void SetStat(Stat stat, int value)
    {
        statsManager[stat] = value;
    }

    public void AddEffect(Effect effect)
    {
        effectTracker.AddEffect(effect);
    }
}
