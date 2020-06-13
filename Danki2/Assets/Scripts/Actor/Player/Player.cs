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

    public EnumDictionary<AbilityReference, int> AbilityInventory { get; private set; }
    
    public AbilityManager AbilityManager { get; private set; }

    public override ActorType Type => ActorType.Player;

    protected override void Awake()
    {
        base.Awake();

        AbilityInventory = new EnumDictionary<AbilityReference, int>(0);
        AbilityInventory[AbilityReference.Bite] = 1;
        AbilityInventory[AbilityReference.Pounce] = 2;
        AbilityInventory[AbilityReference.Slash] = 3;
        AbilityInventory[AbilityReference.Dash] = 4;
        AbilityInventory[AbilityReference.DaggerThrow] = 5;
        AbilityInventory[AbilityReference.Lunge] = 6;
        AbilityInventory[AbilityReference.Smash] = 7;
        AbilityInventory[AbilityReference.Whirlwind] = 9;

        AbilityTree = AbilityTreeFactory.CreateTree(
            AbilityTreeFactory.CreateNode(
                AbilityReference.SweepingStrike,
                AbilityTreeFactory.CreateNode(
                    AbilityReference.Dash,
                    AbilityTreeFactory.CreateNode(AbilityReference.Leap),
                    AbilityTreeFactory.CreateNode(AbilityReference.Smash)
                ),
                AbilityTreeFactory.CreateNode(AbilityReference.Whirlwind)
            ),
            AbilityTreeFactory.CreateNode(
                AbilityReference.Lunge,
                AbilityTreeFactory.CreateNode(AbilityReference.DaggerThrow),
                AbilityTreeFactory.CreateNode(
                    AbilityReference.Whirlwind,
                    null,
                    AbilityTreeFactory.CreateNode(AbilityReference.LeechingStrike)
                )
            )
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
        if (remainingRollCooldown <= 0)
        {
            MovementManager.LockMovement(
                rollDuration,
                GetStat(Stat.Speed) * rollSpeedMultiplier,
                direction,
                direction
            );
            remainingRollCooldown = totalRollCooldown;
            trailRenderer.emitting = true;
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
