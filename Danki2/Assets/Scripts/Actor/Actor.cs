using System;
using System.Collections;
using UnityEngine;

public abstract class Actor : MonoBehaviour
{
    [HideInInspector]
    public Stats baseStats = new Stats(0);
    [HideInInspector]
    public Actor Target = null;

    private StatsManager _statsManager;
    private EffectManager _effectManager;
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
        _effectManager = new EffectManager(this, _statsManager);
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
        _effectManager.ProcessEffects();
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

    /// <summary>
    /// Adds an active effect to the actor, this effect will last for the given duration.
    /// </summary>
    /// <param name="effect"> The effect to add. </param>
    /// <param name="duration"> The duration of the effect. </param>
    public void AddActiveEffect(Effect effect, float duration)
    {
        _effectManager.AddActiveEffect(effect, duration);
    }

    /// <summary>
    /// Adds a passive effect to the actor. The passive effect can be removed using the returned Guid.
    /// </summary>
    /// <param name="effect"> The effect to add. </param>
    /// <returns> The Guid to use to remove the effect. </returns>
    public Guid AddPassiveEffect(Effect effect)
    {
        return _effectManager.AddPassiveEffect(effect);
    }

    /// <summary>
    /// This will remove the effect with the given Guid from the actor. This id is the one returned from
    /// the AddPassiveEffect method.
    /// </summary>
    /// <param name="effectId"> The id of the effect to remove. </param>
    public void RemovePassiveEffect(Guid effectId)
    {
        _effectManager.RemovePassiveEffect(effectId);
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
        _movementManager.LockMovement(duration, speed, direction, rotateForwards, @override, passThrough);
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

    /// <summary>
    /// Fix the rotation we lerp towards on the next frame.
    /// </summary>
    /// <param name="direction"></param>
    public void FixNextRotation(Vector3 direction)
    {
        _movementManager.FixNextRotation(direction);
    }

    /// <summary>
    /// Lock actor's position but not it's rotation.
    /// </summary>
    /// <param name="duration"></param>
    /// <param name="faceDirection">Initial direction to face.</param>
    /// <param name="override"></param>
    public void Root(float duration, Vector3 faceDirection, bool @override = false)
    {
        _movementManager.Root(duration, faceDirection, @override);
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

    public void WaitAndCast(float waitTime, Func<InstantCast> abilityFactory)
    {
        StartCoroutine(
            WaitAndCastCoroutine(waitTime, abilityFactory)
        );
    }
    
    private IEnumerator WaitAndCastCoroutine(float waitTime, Func<InstantCast> abilityFactory)
    {
        yield return new WaitForSeconds(waitTime);
        abilityFactory.Invoke().Cast();
    }

    protected abstract void OnDeath();
}
