using UnityEngine;

public abstract class Actor : MonoBehaviour
{
    [HideInInspector]
    public Stats baseStats = new Stats(0);

    private StatsManager _statsManager;
    private EffectTracker _effectTracker;
    private MovementManager _movementManager;

    protected virtual void Start()
    {
        _statsManager = new StatsManager(baseStats);
        _effectTracker = new EffectTracker(this);

        var rigidBody = gameObject.GetComponent<Rigidbody>();
        _movementManager = new MovementManager(this, rigidBody);
    }

    protected virtual void Update()
    {
        _statsManager.Rebase();
        _effectTracker.ProcessEffects();
        this.Act();
    }

    protected virtual void LateUpdate()
    {
        _movementManager.ExecuteMovement();
    }

    protected abstract void Act();

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

    public void LockMovement(float duration, float speed, Vector3 direction)
    {
        _movementManager.LockMovement(duration, speed, direction);
    }

    protected void MoveAlong(Vector3 vec)
    {
        _movementManager.MoveAlong(vec);
    }

    protected void MoveToward(Vector3 target)
    {
        _movementManager.MoveToward(target);
    }
}
