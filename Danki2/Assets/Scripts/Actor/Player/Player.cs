using UnityEngine;
using System.Collections;

public class Player : Actor
{
    [HideInInspector]
    public float abilityCooldown = 1f;
    [HideInInspector]
    public float totalDashCooldown = 1f;
    [HideInInspector]
    public float dashDuration = 0.2f;
    [HideInInspector]
    public float dashSpeedMultiplier = 3f;
    [HideInInspector]
    public float abilityTimeoutLimit = 5f;

    private float remainingDashCooldown = 0f;

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
        AbilityInventory[AbilityReference.Roll] = 4;
        AbilityInventory[AbilityReference.DaggerThrow] = 5;
        AbilityInventory[AbilityReference.Lunge] = 6;
        AbilityInventory[AbilityReference.Smash] = 7;
        AbilityInventory[AbilityReference.Whirlwind] = 9;

        AbilityTree = AbilityTreeFactory.CreateTree(
            AbilityTreeFactory.CreateNode(
                AbilityReference.Slash,
                AbilityTreeFactory.CreateNode(
                    AbilityReference.Roll,
                    AbilityTreeFactory.CreateNode(AbilityReference.Leap),
                    AbilityTreeFactory.CreateNode(AbilityReference.Smash)
                ),
                AbilityTreeFactory.CreateNode(AbilityReference.Whirlwind)
            ),
            AbilityTreeFactory.CreateNode(
                AbilityReference.Lunge,
                AbilityTreeFactory.CreateNode(AbilityReference.DaggerThrow),
                AbilityTreeFactory.CreateNode(AbilityReference.Whirlwind)
            )
        );

        RegisterAbilityDataDiffer(new AbilityDataOrbsDiffer(AbilityTree));
        SetAbilityBonusCalculator(new AbilityBonusOrbsCalculator(AbilityTree));
        AbilityManager = new AbilityManager(this, abilityTimeoutLimit, abilityCooldown, updateSubject, lateUpdateSubject);
    }

    protected void Start()
    {
        gameObject.tag = Tags.Player;
    }

    protected override void Update()
    {
        base.Update();

        TickDashCooldown();
    }

    public void Dash(Vector3 direction)
    {
        if (remainingDashCooldown <= 0)
        {
            MovementManager.LockMovement(
                dashDuration,
                GetStat(Stat.Speed) * dashSpeedMultiplier,
                direction,
                direction
            );
            remainingDashCooldown = totalDashCooldown;
            trailRenderer.emitting = true;
            StartCoroutine(EndDashVisualAfterDelay());
        }
    }

    public void PlayWhiffSound()
    {
        whiffAudio.Play();
    }

    protected override void OnDeath()
    {
        // TODO: Implement Player death.
        Debug.Log("The player died");
    }

    private void TickDashCooldown()
    {
        remainingDashCooldown = Mathf.Max(0f, remainingDashCooldown - Time.deltaTime);
    }

    private IEnumerator EndDashVisualAfterDelay()
    {
        yield return new WaitForSeconds(dashDuration * 2);
        trailRenderer.emitting = false;
    }
}
