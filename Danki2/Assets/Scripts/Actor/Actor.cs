using UnityEngine;

public abstract class Actor : MonoBehaviour
{
    [HideInInspector]
    public Stats baseStats = new Stats(0);

    private StatsManager _statsManager;

    private EffectTracker _effectTracker;

    protected virtual void Start()
    {
        _statsManager = new StatsManager(baseStats);
        _effectTracker = new EffectTracker(this);
        _statsManager.RegisterPipe((stat, value) => _effectTracker.ProcessStat(stat, value));
    }

    protected virtual void Update()
    {
        _effectTracker.ProcessEffects();
        this.Act();
    }

    protected abstract void Act();

    public float GetStat(Stat stat)
    {
        return _statsManager[stat];
    }

    public void AddEffect(Effect effect)
    {
        _effectTracker.AddEffect(effect);
    }

    protected void MoveAlongVector(Vector3 vec)
    {
        if (vec == Vector3.zero)
        {
            return;
        }

        vec.Normalize();
        var speed = _statsManager[Stat.Speed];

        transform.Translate(vec * Time.deltaTime * speed);
    }

    protected void MoveToward(Vector3 target)
    {
        var vecToMove = target - transform.position;
        MoveAlongVector(vecToMove);
    }
}
