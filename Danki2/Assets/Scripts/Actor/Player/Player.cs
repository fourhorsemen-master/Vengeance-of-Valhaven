using UnityEngine;

public class Player : Actor
{
    public override ActorType Type => ActorType.Player;

    // This is no readonly as the editor needs to updates it. Seems safer than risking individual settings being updated.
    public PlayerSettings Settings { get; set; } = new PlayerSettings();

    private float remainingRollCooldown = 0f;

    // Components
    [SerializeField]
    private AudioSource whiffAudio = null;
    [SerializeField]
    private AudioSource rollAudio = null;

    // Services
    public AbilityTree AbilityTree { get; private set; }    
    public AbilityManager AbilityManager { get; private set; }
    public PlayerTargetFinder TargetFinder { get; private set; }
    
    // Subjects
    public Subject RollSubject { get; } = new Subject();

    protected override void Awake()
    {
        base.Awake();

        EnumDictionary<AbilityReference, int> ownedAbilities = new EnumDictionary<AbilityReference, int>(3);

        AbilityTree = AbilityTreeFactory.CreateTree(
            ownedAbilities,
            AbilityTreeFactory.CreateNode(AbilityReference.SweepingStrike),
            AbilityTreeFactory.CreateNode(AbilityReference.Lunge)
        );

        RegisterAbilityDataDiffer(new AbilityDataOrbsDiffer(AbilityTree));
        SetAbilityBonusCalculator(new AbilityBonusOrbsCalculator(AbilityTree));
        AbilityManager = new AbilityManager(this, updateSubject, lateUpdateSubject);
        TargetFinder = new PlayerTargetFinder(this, updateSubject);
    }

    protected override void Start()
    {
        base.Start();
        
        gameObject.tag = Tags.Player;
    }

    protected override void Update()
    {
        base.Update();

        TickRollCooldown();
    }

    public void Roll(Vector3 direction)
    {
        if (remainingRollCooldown > 0 || ChannelService.Active) return;

        bool rolled = MovementManager.TryLockMovement(
            MovementLockType.Dash,
            Settings.RollDuration,
            GetStat(Stat.Speed) * Settings.RollSpeedMultiplier,
            direction,
            direction
        );

        if (rolled)
        {
            remainingRollCooldown = Settings.TotalRollCooldown;
            rollAudio.Play();
            RollSubject.Next();
            StartTrail(Settings.RollDuration * 2);
        }
    }

    public void PlayWhiffSound()
    {
        whiffAudio.Play();
    }

    private void TickRollCooldown()
    {
        remainingRollCooldown = Mathf.Max(0f, remainingRollCooldown - Time.deltaTime);
    }
}
