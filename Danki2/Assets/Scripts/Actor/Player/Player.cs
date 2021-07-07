using FMODUnity;
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

    [Header("Fmod events")]
    [EventRef] [SerializeField] private string whiffEvent = null;

    private bool readyToRoll = true;

    private const float MovementMultiplier = 0.01f;

    public float ShortCooldown => shortCooldown;
    public float LongCooldown => longCooldown;
    public float ComboTimeout => comboTimeout;

    // Services
    public AbilityTree AbilityTree { get; private set; }
    public ComboManager ComboManager { get; private set; }
    public PlayerTargetFinder TargetFinder { get; private set; }
    public RuneManager RuneManager { get; private set; }
    public CurrencyManager CurrencyManager { get; private set; }
    
    // Subjects
    public Subject RollSubject { get; } = new Subject();
    public Subject<bool> AbilityFeedbackSubject { get; } = new Subject<bool>();

    public FeedbackStatus FeedbackSinceLastCast { get; private set; } = FeedbackStatus.Waiting;

    protected override Tag Tag => Tag.Player;

    protected override void Awake()
    {
        base.Awake();

        AbilityTree = PersistenceManager.Instance.SaveData.SerializableAbilityTree.Deserialize();
        ComboManager = new ComboManager(this, updateSubject, rollResetsCombo);
        TargetFinder = new PlayerTargetFinder(this, updateSubject);
        RuneManager = new RuneManager(this);
        CurrencyManager = new CurrencyManager();

        InstantCastService.SetFeedbackTimeout(feedbackTimeout);
        ChannelService.SetFeedbackTimeout(feedbackTimeout);

        InstantCastService.FeedbackSubject.Subscribe(feedback => AbilityFeedbackSubject.Next(feedback));
        ChannelService.FeedbackSubject.Subscribe(feedback => AbilityFeedbackSubject.Next(feedback));

        AbilityFeedbackSubject.Subscribe(feedback => FeedbackSinceLastCast = feedback ? FeedbackStatus.Succeeded : FeedbackStatus.Failed);
        ComboManager.SubscribeToStateEntry(ComboState.ReadyAtRoot, () => FeedbackSinceLastCast = FeedbackStatus.Waiting);
        ComboManager.SubscribeToStateEntry(ComboState.ReadyInCombo, () => FeedbackSinceLastCast = FeedbackStatus.Waiting);

        SetAbilityBonusCalculator(new AbilityBonusTreeDepthCalculator(AbilityTree));
    }

    public void Roll(Vector3 direction)
    {
        if (!readyToRoll || ChannelService.Active) return;

        bool rolled = MovementManager.TryLockMovement(
            MovementLockType.Dash,
            rollDuration,
            StatsManager.Get(Stat.Speed) * rollSpeedMultiplier * MovementMultiplier,
            direction,
            direction
        );

        if (rolled)
        {
            // FMOD_TODO: play roll event here
            RollSubject.Next();
            StartTrail(rollDuration * 2);

            readyToRoll = false;
            this.WaitAndAct(totalRollCooldown, () => readyToRoll = true);

            AnimController.Play("Dash_OneShot");
        }
    }

    public void PlayWhiffSound() => RuntimeManager.PlayOneShot(whiffEvent);

    protected override void OnDeath()
    {
        base.OnDeath();
        PersistenceManager.Instance.TransitionToDefeatRoom();
    }
}
