using Assets.Scripts.AI;
using Assets.Scripts.Effects;
using UnityEngine;

public abstract class Actor : MonoBehaviour
{
    [HideInInspector]
    public Stats baseStats = new Stats(0);

    private StatsManager _statsManager;

    private EffectTracker _effectTracker;

    public abstract IAI AI { get; }

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
        this.Act();
    }

    protected virtual void Act()
    {
        this.AI.Act();
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

    protected void MoveAlongVector(Vector3 vec)
    {
        vec.Normalize();
        vec *= _statsManager[Stat.Speed];
        transform.Translate(vec);
    }

    protected void MoveToward(Vector3 target)
    {
        var vecToMove = target - transform.position;
        MoveAlongVector(vecToMove);
    }
}
