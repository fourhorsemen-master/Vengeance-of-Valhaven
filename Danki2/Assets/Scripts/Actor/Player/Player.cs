using UnityEngine;
using System.Collections;

public class Player : Actor
{
    [HideInInspector]
    public float abilityCooldown = 1f;
    [HideInInspector]
    public float totalRollCooldown = 1f;
    [HideInInspector]
    public float rollDuration = 0.2f;
    [HideInInspector]
    public float rollSpeedMultiplier = 3f;
    [HideInInspector]
    public float abilityTimeoutLimit = 5f;

    private float remainingRollCooldown = 0f;

    [SerializeField]
    private TrailRenderer trailRenderer = null;

    [SerializeField]
    private AudioSource whiffAudio = null;

    public AbilityTree AbilityTree { get; private set; }
    
    public AbilityManager AbilityManager { get; private set; }

    public override ActorType Type => ActorType.Player;
    
    public Subject RollSubject { get; } = new Subject();

    protected override void Awake()
    {
        base.Awake();

        EnumDictionary<AbilityReference, int> ownedAbilities = new EnumDictionary<AbilityReference, int>(0);
        ownedAbilities[AbilityReference.Bash] = 3;
        ownedAbilities[AbilityReference.DaggerThrow] = 3;
        ownedAbilities[AbilityReference.Dash] = 3;
        ownedAbilities[AbilityReference.Leap] = 3;
        ownedAbilities[AbilityReference.LeechingStrike] = 3;
        ownedAbilities[AbilityReference.Lunge] = 3;
        ownedAbilities[AbilityReference.Slash] = 3;
        ownedAbilities[AbilityReference.Smash] = 3;
        ownedAbilities[AbilityReference.SweepingStrike] = 3;
        ownedAbilities[AbilityReference.Sprint] = 3;
        ownedAbilities[AbilityReference.Whirlwind] = 3;
        ownedAbilities[AbilityReference.Hook] = 3;
        ownedAbilities[AbilityReference.Backstab] = 3;
        ownedAbilities[AbilityReference.PiercingRush] = 3;

        AbilityTree = AbilityTreeFactory.CreateTree(
            ownedAbilities,
            AbilityTreeFactory.CreateNode(AbilityReference.SweepingStrike),
            AbilityTreeFactory.CreateNode(AbilityReference.Lunge)
        );

        RegisterAbilityDataDiffer(new AbilityDataOrbsDiffer(AbilityTree));
        SetAbilityBonusCalculator(new AbilityBonusOrbsCalculator(AbilityTree));
        AbilityManager = new AbilityManager(this, abilityTimeoutLimit, abilityCooldown, updateSubject, lateUpdateSubject);
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

        var rolled = MovementManager.TryLockMovement(
            MovementLockType.Dash,
            rollDuration,
            GetStat(Stat.Speed) * rollSpeedMultiplier,
            direction,
            direction
        );

        if (rolled)
        {
            remainingRollCooldown = totalRollCooldown;
            trailRenderer.emitting = true;
            RollSubject.Next();
            StartCoroutine(EndRollVisualAfterDelay());
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

    private IEnumerator EndRollVisualAfterDelay()
    {
        yield return new WaitForSeconds(rollDuration * 2);
        trailRenderer.emitting = false;
    }
}
