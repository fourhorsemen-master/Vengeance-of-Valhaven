using UnityEngine;

public class Actor : MonoBehaviour
{
    [HideInInspector]
    public Stats baseStats = new Stats(0);

    private StatsManager _statsManager;

    private EffectTracker _effectTracker;

    // Start is called before the first frame update
    void Start()
    {
        _statsManager = new StatsManager(baseStats);
        _effectTracker = new EffectTracker(this);
    }

    // Update is called once per frame
    void Update()
    {
        _statsManager.Rebase();
        _effectTracker.ProcessEffects();
    }

    public int GetStat(Stat stat)
    {
        return _statsManager[stat];
    }

    public void SetStat(Stat stat, int value)
    {
        _statsManager[stat] = value;
    }

    public void AddEffect(Effect effect)
    {
        _effectTracker.AddEffect(effect);
    }
}
