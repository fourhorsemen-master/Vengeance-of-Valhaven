using UnityEngine;

public abstract class Actor : MonoBehaviour
{
    [HideInInspector]
    public Stats baseStats = new Stats(0);

    private StatsManager _statsManager;
    private EffectTracker _effectTracker;
    private MovementManager _movementManager;
    protected ChannelService _channelService;

    private float _health;
    public int Health => Mathf.CeilToInt(_health);
    public bool Dead { get; private set; }
    public bool IsDamaged => Health < GetStat(Stat.MaxHealth);
    public float RemainingChannelDuration => _channelService.RemainingDuration;
    public float TotalChannelDuration => _channelService.TotalDuration;

    public abstract ActorType Type { get; }

    protected virtual void Awake()
    {
        _statsManager = new StatsManager(baseStats);
        _effectTracker = new EffectTracker(this, _statsManager);
        _channelService = new ChannelService();

        _health = GetStat(Stat.MaxHealth);
        Dead = false;
    }

    protected virtual void Start()
    {
        var rigidBody = gameObject.GetComponent<Rigidbody>();
        _movementManager = new MovementManager(this, rigidBody);
    }

    protected virtual void Update()
    {
        _effectTracker.ProcessEffects();
        _channelService.Update();

        if (_health <= 0f && !Dead)
        {
            OnDeath();
            Dead = true;
        }
    }

    protected virtual void LateUpdate()
    {
        if (!Dead)
        {
            _movementManager.ExecuteMovement();
        }
    }

    public int GetStat(Stat stat)
    {
        return _statsManager[stat];
    }

    public void AddEffect(Effect effect)
    {
        _effectTracker.AddEffect(effect);
    }

    /// <summary>
    /// Lock the Actors movement for a duration.
    /// </summary>
    /// <param name="duration"></param>
    /// <param name="speed"></param>
    /// <param name="direction"></param>
    /// <param name="override">Whether to override any existing movement lock. Defaults to false.</param>
    public void LockMovement(float duration, float speed, Vector3 direction, bool @override = false, bool passThrough = false)
    {
        _movementManager.LockMovement(duration, speed, direction, @override, passThrough);
    }

    /// <summary>
    /// Moves the actor with the direction of the provided direction vector.
    /// </summary>
    /// <param name="vec"></param>
    public void MoveAlong(Vector3 vec)
    {
        _movementManager.MoveAlong(vec);
    }

    /// <summary>
    /// Moves the actor toward the provided position vector.
    /// </summary>
    /// <param name="vec"></param>
    public void MoveToward(Vector3 target)
    {
        _movementManager.MoveToward(target);
    }

    public void ModifyHealth(float healthChange)
    {
        if (Dead) return;

        _health = Mathf.Min(_health + healthChange, GetStat(Stat.MaxHealth));
    }

    public void StartChannel(Channel channel)
    {
        _channelService.Start(channel);
    }

    public void CancelChannel()
    {
        _channelService.Cancel();
    }
        
    public bool Opposes(Actor target)
    {
        return tag != target.tag;
    }

    protected abstract void OnDeath();
}
