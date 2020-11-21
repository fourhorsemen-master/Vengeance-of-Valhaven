using UnityEngine;

public class Player : Actor
{
    public override ActorType Type => ActorType.Player;

    [Header("Ability tree")]
    [SerializeField] private float shortCooldown = 0.75f;
    [SerializeField] private float longCooldown = 1.5f;
    [SerializeField] private float comboTimeout = 2f;
    [SerializeField] private float feedbackTimeout = 1f;
    [SerializeField] private bool rollResetsCombo = false;

    [Header("Roll")]
    [SerializeField] private float totalRollCooldown = 1f;
    [SerializeField] private float rollDuration = 0.3f;
    [SerializeField] private float rollSpeedMultiplier = 2f;

    [Header("Audio")]
    [SerializeField] private AudioSource whiffAudio = null;
    [SerializeField] private AudioSource rollAudio = null;

    public float ShortCooldown => shortCooldown;
    public float LongCooldown => longCooldown;
    public float ComboTimeout => comboTimeout;
    public float FeedbackTimeout => feedbackTimeout;
    public bool RollResetsCombo => rollResetsCombo;

    private bool readyToRoll = true;

    private Subscription<bool> abilityFeedbackSubscription;

    public Direction LastCastDirection { get; private set; }
    public bool? FeedbackSinceLastCast { get; private set; } = null;

    // Services
    public AbilityTree AbilityTree { get; private set; }
    public PlayerTargetFinder TargetFinder { get; private set; }
    
    // Subjects
    public Subject RollSubject { get; } = new Subject();
    public Subject<Direction> ChannelStartSubject { get; } = new Subject<Direction>();
    public Subject<bool> AbilityFeedbackSubject { get; } = new Subject<bool>();
    public Subject ComboCompleteSubject { get; } = new Subject();
    public Subject ComboFailedSubject { get; } = new Subject();
    public Subject ComboContinueSubject { get; } = new Subject();
    public Subject WhiffSubject { get; } = new Subject();
    public Subject ReadyToCastSubject { get; } = new Subject();

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

        TargetFinder = new PlayerTargetFinder(this, updateSubject);
    }

    protected override void Start()
    {
        base.Start();
        
        gameObject.tag = Tags.Player;
    }

    public void Ready()
    {
        FeedbackSinceLastCast = null;
        ReadyToCastSubject.Next();
    }

    public void Cast(Direction direction)
    {
        if (!AbilityTree.CanWalkDirection(direction)) return;
        LastCastDirection = direction;

        AbilityReference abilityReference = AbilityTree.GetAbility(direction);

        AbilityType abilityType = AbilityLookup.Instance.GetAbilityType(abilityReference);

        bool hasCast = false;

        if (abilityType == AbilityType.InstantCast)
        {
            hasCast = InstantCastService.TryCast(
                abilityReference,
                TargetFinder.FloorTargetPosition,
                TargetFinder.OffsetTargetPosition,
                subject => SubscribeToFeedback(subject),
                TargetFinder.Target
            );
        }
        else if (abilityType == AbilityType.Channel)
        {
            hasCast = ChannelService.TryStartChannel(
                abilityReference,
                subject => SubscribeToFeedback(subject)
            );
            if (hasCast)
            {
                ChannelStartSubject.Next(direction);
            }
        }

        if (hasCast)
        {
            AbilityTree.Walk(LastCastDirection);
        }
    }

    public void CompleteCombo()
    {
        AbilityTree.Reset();
        ComboCompleteSubject.Next();
    }

    public void ContinueCombo()
    {
        ComboContinueSubject.Next();
    }

    public void FailCombo()
    {
        AbilityTree.Reset();
        PlayWhiffSound();
        ComboFailedSubject.Next();
    }

    public void Whiff()
    {
        AbilityTree.Reset();
        PlayWhiffSound();
        WhiffSubject.Next();
    }

    public void Roll(Vector3 direction)
    {
        if (!readyToRoll || ChannelService.Active) return;

        bool rolled = MovementManager.TryLockMovement(
            MovementLockType.Dash,
            rollDuration,
            GetStat(Stat.Speed) * rollSpeedMultiplier,
            direction,
            direction
        );

        if (rolled)
        {
            rollAudio.Play();
            RollSubject.Next();
            StartTrail(rollDuration * 2);

            readyToRoll = false;
            this.WaitAndAct(totalRollCooldown, () => readyToRoll = true);
        }
    }

    private void PlayWhiffSound()
    {
        whiffAudio.Play();
    }

    private void SubscribeToFeedback(Subject<bool> subject)
    {
        abilityFeedbackSubscription = subject.Subscribe(feedback =>
        {
            abilityFeedbackSubscription.Unsubscribe();
            FeedbackSinceLastCast = feedback;
            AbilityFeedbackSubject.Next(feedback);
        });
    }
}
