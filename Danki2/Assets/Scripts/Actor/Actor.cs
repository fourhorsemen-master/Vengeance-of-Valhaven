using UnityEngine;

public abstract class Actor : MonoBehaviour
{
    [HideInInspector]
    public Stats baseStats = new Stats(0);

    private StatsManager _statsManager;
    private EffectTracker _effectTracker;
    private MovementManager _movementManager;

    protected virtual void Awake()
    {
        _statsManager = new StatsManager(baseStats);
        _effectTracker = new EffectTracker(this, _statsManager);
    }

    protected virtual void Start()
    {
        var rigidBody = gameObject.GetComponent<Rigidbody>();
        _movementManager = new MovementManager(this, rigidBody);
    }

    protected virtual void Update()
    {
        _effectTracker.ProcessEffects();
    }

    protected virtual void LateUpdate()
    {
        _movementManager.ExecuteMovement();
    }

    public int GetStat(Stat stat)
    {
        return _statsManager[stat];
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
