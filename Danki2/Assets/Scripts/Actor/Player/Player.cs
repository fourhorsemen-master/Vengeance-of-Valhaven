using System;
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

    public AbilityTree AbilityTree { get; private set; }

    public AbilityManager AbilityManager { get; private set; }

    public override ActorType Type => ActorType.Player;

    protected override void Awake()
    {
        base.Awake();

        AbilityTree = AbilityTreeFactory.CreateTree(
            AbilityTreeFactory.CreateNode(
                AbilityReference.Slash,
                AbilityTreeFactory.CreateNode(
                    AbilityReference.Roll,
                    rightChild: AbilityTreeFactory.CreateNode(AbilityReference.Smash)
                ),
                AbilityTreeFactory.CreateNode(AbilityReference.Whirlwind)
            ),
            AbilityTreeFactory.CreateNode(
                AbilityReference.Lunge,
                AbilityTreeFactory.CreateNode(AbilityReference.DaggerThrow),
                AbilityTreeFactory.CreateNode(AbilityReference.Whirlwind)
            )
        );

        AbilityManager = new AbilityManager(this, abilityTimeoutLimit, abilityCooldown, updateSubject, lateUpdateSubject);
    }

    protected void Start()
    {
        this.gameObject.tag = Tags.Player;
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
