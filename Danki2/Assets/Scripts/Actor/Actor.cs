using UnityEngine;

public abstract class Actor : MonoBehaviour
{
    [HideInInspector]
    public StatsDictionary baseStats = new StatsDictionary(0);

    private StatsManager statsManager;
    private MovementManager movementManager;
    private Subject updateSubject = new Subject();

    private float health;

    public ChannelService ChannelService { get; private set; }
    public EffectManager EffectManager { get; private set; }
    public Actor Target { get; set; } = null;
    public int Health => Mathf.CeilToInt(health);
    public bool Dead { get; private set; }
    public bool IsDamaged => Health < GetStat(Stat.MaxHealth);

    public abstract ActorType Type { get; }

    protected virtual void Awake()
    {
        this.statsManager = new StatsManager(baseStats);
        EffectManager = new EffectManager(this, updateSubject, statsManager);
        ChannelService = new ChannelService(updateSubject);

        health = GetStat(Stat.MaxHealth);
        Dead = false;
    }

    protected virtual void Start()
    {
        Rigidbody rigidBody = gameObject.GetComponent<Rigidbody>();
        this.movementManager = new MovementManager(this, rigidBody);
    }

    protected virtual void Update()
    {
        if (health <= 0f && !Dead)
        {
            OnDeath();
            Dead = true;
        }

        if (Dead) return;

        this.updateSubject.Next();
    }

    protected virtual void LateUpdate()
    {
        if (!Dead)
        {
            this.movementManager.ExecuteMovement();
        }
    }

    public int GetStat(Stat stat)
    {
        return statsManager[stat];
    }

    /// <summary>
    /// Lock the Actors movement for a duration.
    /// </summary>
    /// <param name="duration"></param>
    /// <param name="speed"></param>
    /// <param name="direction"></param>
    /// <param name="rotateForwards"></param>
    /// <param name="override">Whether to override any existing movement lock. Defaults to false.</param>
    /// <param name="passThrough"></param>
    public void LockMovement(float duration, float speed, Vector3 direction, bool rotateForwards = true, bool @override = false, bool passThrough = false)
    {
        this.movementManager.LockMovement(duration, speed, direction, rotateForwards, @override, passThrough);
    }

    /// <summary>
    /// Moves the actor with the direction of the provided direction vector.
    /// </summary>
    /// <param name="vec"></param>
    public void MoveAlong(Vector3 vec)
    {
        this.movementManager.MoveAlong(vec);
    }

    /// <summary>
    /// Moves the actor toward the provided position vector.
    /// </summary>
    /// <param name="vec"></param>
    public void MoveToward(Vector3 target)
    {
        this.movementManager.MoveToward(target);
    }

    /// <summary>
    /// Fix the rotation we lerp towards on the next frame.
    /// </summary>
    /// <param name="direction"></param>
    public void FixNextRotation(Vector3 direction)
    {
        this.movementManager.FixNextRotation(direction);
    }

    /// <summary>
    /// Lock actor's position but not it's rotation.
    /// </summary>
    /// <param name="duration"></param>
    /// <param name="faceDirection">Initial direction to face.</param>
    /// <param name="override"></param>
    public void Root(float duration, Vector3 faceDirection, bool @override = false)
    {
        this.movementManager.Root(duration, faceDirection, @override);
    }

    public void ModifyHealth(float healthChange)
    {
        if (Dead) return;

        health = Mathf.Min(health + healthChange, GetStat(Stat.MaxHealth));
    }
        
    public bool Opposes(Actor target)
    {
        return tag != target.tag;
    }

    protected abstract void OnDeath();
}
