using System;
using UnityEngine;
using UnityEngine.AI;

public abstract class Actor : MonoBehaviour
{
    [HideInInspector]
    public StatsDictionary baseStats = new StatsDictionary(0);

    [SerializeField]
    private NavMeshAgent navmeshAgent = null;

    [SerializeField]
    private TrailRenderer trailRenderer = null;

    [SerializeField]
    private MeshRenderer[] meshRenderers = null;

    // Serialized properties
    [SerializeField] private float weight = 0;
    public float Weight => weight;

    [Header("Sockets")]

    [SerializeField] private Transform centre = null;
    public Vector3 Centre => centre.position;

    [SerializeField] private Transform abilitySource = null;
    public Vector3 AbilitySource => abilitySource.position;

    [SerializeField] private Transform collisionTemplateSource = null;
    public Vector3 CollisionTemplateSource => collisionTemplateSource.position;

    protected readonly Subject startSubject = new Subject();
    protected readonly Subject updateSubject = new Subject();
    protected readonly Subject lateUpdateSubject = new Subject();

    private Coroutine stopTrailCoroutine;

    // Services
    public StatsManager StatsManager { get; private set; }
    public HealthManager HealthManager { get; private set; }
    public ChannelService ChannelService { get; private set; }
    public InstantCastService InstantCastService { get; private set; }
    public EffectManager EffectManager { get; private set; }
    public MovementManager MovementManager { get; private set; }
    public InterruptionManager InterruptionManager { get; private set; }
    public HighlightManager HightlightManager { get; private set; }

    public bool Dead { get; private set; }
    public Subject DeathSubject { get; } = new Subject();
    public abstract ActorType Type { get; }

    public bool CanCast => !Dead && !MovementManager.Stunned && !MovementManager.MovementLocked;
    public float CastableTimeElapsed { get; private set; } = 0f;

    protected virtual void Awake()
    {
        ActorCache.Instance.Register(this);
        
        gameObject.SetLayerRecursively(Layer.Actors);

        StatsManager = new StatsManager(baseStats);
        EffectManager = new EffectManager(this, updateSubject);
        HealthManager = new HealthManager(this, updateSubject);
        InterruptionManager = new InterruptionManager(this, startSubject, updateSubject);
        ChannelService = new ChannelService(this, startSubject, lateUpdateSubject);
        InstantCastService = new InstantCastService(this);
        MovementManager = new MovementManager(this, updateSubject, navmeshAgent);
        HightlightManager = new HighlightManager(updateSubject, meshRenderers);

        AbilityDataStatsDiffer abilityDataStatsDiffer = new AbilityDataStatsDiffer(this);
        RegisterAbilityDataDiffer(abilityDataStatsDiffer);

        Dead = false;
    }

    protected virtual void Start()
    {
        startSubject.Next();
    }

    protected virtual void Update()
    {
        updateSubject.Next();

        if (CanCast) CastableTimeElapsed += Time.deltaTime;
        else CastableTimeElapsed = 0f;
    }

    protected virtual void LateUpdate()
    {
        lateUpdateSubject.Next();

        if (HealthManager.Health <= 0 && !Dead)
        {
            OnDeath();
        }
    }
        
    public bool Opposes(Actor target)
    {
        return !CompareTag(target.tag);
    }

    public void InterruptibleAction(float delay, InterruptionType interruptionType, Action action)
    {
        Coroutine coroutine = this.WaitAndAct(delay, action);
        
        // We don't need to worry about deregistering the interruptible as Stopping a finished coroutine doesn't cause any problems.
        InterruptionManager.Register(
            interruptionType,
            () => StopCoroutine(coroutine),
            InterruptibleFeature.InterruptOnDeath
        );
    }

    public void InterruptibleIntervalAction(float interval, InterruptionType interruptionType, Action action, float startDelay = 0, int? numRepetitions = null)
    {
        Coroutine coroutine = this.ActOnInterval(interval, action, startDelay, numRepetitions);

        // We don't need to worry about deregistering the interruptible as Stopping a finished coroutine doesn't cause any problems.
        InterruptionManager.Register(
            interruptionType,
            () => StopCoroutine(coroutine),
            InterruptibleFeature.InterruptOnDeath
        );
    }

    public void StartTrail(float duration)
    {
        trailRenderer.emitting = true;

        if (stopTrailCoroutine != null)
        {
            StopCoroutine(stopTrailCoroutine);
        }

        stopTrailCoroutine = this.WaitAndAct(duration, () => trailRenderer.emitting = false);
    }

    protected virtual void OnDeath()
    {
        Debug.Log($"{tag} died");

        DeathSubject.Next();
        Dead = true;
    }

    protected void RegisterAbilityDataDiffer(IAbilityDataDiffer abilityDataDiffer)
    {
        ChannelService.RegisterAbilityDataDiffer(abilityDataDiffer);
        InstantCastService.RegisterAbilityDataDiffer(abilityDataDiffer);
    }

    protected void SetAbilityBonusCalculator(IAbilityBonusCalculator abilityBonusCalculator)
    {
        ChannelService.SetAbilityBonusCalculator(abilityBonusCalculator);
        InstantCastService.SetAbilityBonusCalculator(abilityBonusCalculator);
    }
}
