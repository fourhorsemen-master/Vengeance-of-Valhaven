using UnityEngine;
using UnityEngine.AI;

public abstract class Actor : MonoBehaviour
{
    [HideInInspector]
    public StatsDictionary baseStats = new StatsDictionary(0);

    [SerializeField] protected NavMeshAgent navmeshAgent = null;

    [SerializeField] private TrailRenderer trailRenderer = null;

    [SerializeField] private Renderer[] meshRenderers = null;

    // Serialized properties
    [SerializeField] private float weight = 0;
    public float Weight => weight;

    [SerializeField] private float rotationSmoothing = 0;
    public float RotationSmoothing => rotationSmoothing;

    [SerializeField] private Animator animController = null;
    public Animator AnimController => animController;

    [SerializeField] private Collider[] colliders = null;
    public Collider[] Colliders => colliders;

    [Header("Sockets")]

    [SerializeField] private Transform centre = null;
    public Transform CentreTransform => centre;
    public Vector3 Centre => centre.position;
    public float Height => Centre.y - transform.position.y;

    [SerializeField] private Transform abilitySource = null;
    public Transform AbilitySourceTransform => abilitySource;
    public Vector3 AbilitySource => abilitySource.position;

    [SerializeField] private Transform collisionTemplateSource = null;
    public Transform CollisionTemplateSourceTransform => collisionTemplateSource;
    public Vector3 CollisionTemplateSource => collisionTemplateSource.position;

    protected readonly Subject startSubject = new Subject();
    protected readonly Subject updateSubject = new Subject();
    protected readonly Subject lateUpdateSubject = new Subject();

    private Coroutine stopTrailCoroutine;

    // Services
    public StatsManager StatsManager { get; private set; }
    public HealthManager HealthManager { get; private set; }
    public EffectManager EffectManager { get; private set; }
    public HighlightManager HighlightManager { get; private set; }

    public bool Dead => HealthManager.Dead;
    public Subject<DeathData> DeathSubject = new Subject<DeathData>();
    public abstract ActorType Type { get; }
    protected abstract Tag Tag { get; }

    public bool IsPlayer => Tag == Tag.Player;

    protected virtual void Awake()
    {
        ActorCache.Instance.Register(this);
        
        gameObject.SetLayerRecursively(Layer.Actors);

        gameObject.SetTag(Tag);

        StatsManager = new StatsManager(baseStats);
        EffectManager = new EffectManager(this, updateSubject);
        HealthManager = new HealthManager(this, updateSubject);
        HighlightManager = new HighlightManager(updateSubject, meshRenderers);
    }

    protected virtual void Start() => startSubject.Next();

    protected virtual void Update() => updateSubject.Next();

    protected virtual void LateUpdate() => lateUpdateSubject.Next();
        
    public bool Opposes(Actor target) => !CompareTag(target.tag);

    public void StartTrail(float duration)
    {
        trailRenderer.emitting = true;

        if (stopTrailCoroutine != null)
        {
            StopCoroutine(stopTrailCoroutine);
        }

        stopTrailCoroutine = this.WaitAndAct(duration, () => trailRenderer.emitting = false);
    }
}
