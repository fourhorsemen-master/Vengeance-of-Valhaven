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

    private ActionControlState previousActionControlState = ActionControlState.None;
    private ActionControlState currentActionControlState = ActionControlState.None;

    private Coroutine abilityTimeout;
    
    [SerializeField]
    private TrailRenderer trailRenderer = null;

    [HideInInspector]
    public float RemainingAbilityCooldown { get; private set; } = 0f;
    [HideInInspector]
    public CastingStatus CastingStatus { get; private set; } = CastingStatus.Ready;

    public AbilityTree AbilityTree { get; private set; }

    public override ActorType Type => ActorType.Player;

    protected override void Awake()
    {
        base.Awake();

        AbilityTree = AbilityTreeFactory.CreateTree(
            AbilityTreeFactory.CreateNode(
                new Slash(),
                AbilityTreeFactory.CreateNode(
                    new Roll(),
                    rightChild: AbilityTreeFactory.CreateNode(new Smash())
                ),
                AbilityTreeFactory.CreateNode(new Whirlwind())
            ),
            AbilityTreeFactory.CreateNode(
                new Lunge(),
                AbilityTreeFactory.CreateNode(new DaggerThrow()),
                AbilityTreeFactory.CreateNode(new Whirlwind())
            )
        );
    }

    protected override void Start()
    {
        base.Start();

        this.gameObject.tag = Tags.Player;

        AbilityTimeoutSubscription();
    }

    protected override void Update()
    {
        ChannelTarget = MouseGamePositionFinder.Instance.GetMouseGamePosition();
        base.Update();

        TickDashCooldown();
        TickAbilityCooldown();
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();

        HandleAbilities();
    }

    public void Dash(Vector3 direction)
    {
        if (remainingDashCooldown <= 0)
        {
            LockMovement(dashDuration, GetStat(Stat.Speed) * dashSpeedMultiplier, direction);
            remainingDashCooldown = totalDashCooldown;
            trailRenderer.emitting = true;
            StartCoroutine(EndDashVisualAfterDelay());
        }
    }

    public void SetCurrentControlState(ActionControlState controlState)
    {
        currentActionControlState = controlState;
    }

    public void SubscribeToTreeWalk(Action<Node> callback)
    {
        AbilityTree.TreeWalkSubject.Subscribe(callback);
    }

    protected override void OnDeath()
    {
        // TODO: Implement Player death.
        Debug.Log("The player died");
    }

    private void AbilityTimeoutSubscription()
    {
        AbilityTree.CurrentDepthSubject.Subscribe((int treeDepth) =>
        {
            if (abilityTimeout != null)
            {
                StopCoroutine(abilityTimeout);
            }

            if (treeDepth > 0)
            {
                abilityTimeout = StartCoroutine(AbilityTimeoutCounter());
            }
        });
    }

    private IEnumerator AbilityTimeoutCounter()
    {
        yield return new WaitForSeconds(abilityTimeoutLimit);
        AbilityTree.Reset();
    }

    private void TickDashCooldown()
    {
        remainingDashCooldown = Mathf.Max(0f, remainingDashCooldown - Time.deltaTime);
    }

    private void TickAbilityCooldown()
    {
        RemainingAbilityCooldown = Mathf.Max(0f, RemainingAbilityCooldown - Time.deltaTime);
        if (RemainingAbilityCooldown == 0f && CastingStatus == CastingStatus.Cooldown)
        {
            CastingStatus = CastingStatus.Ready;
        }
    }

    private IEnumerator EndDashVisualAfterDelay()
    {
        yield return new WaitForSeconds(dashDuration * 2);
        trailRenderer.emitting = false;
    }

    private void HandleAbilities()
    {
        CastingCommand castingCommand = ControlMatrix.GetCastingCommand(
            CastingStatus,
            previousActionControlState,
            currentActionControlState
        );

        previousActionControlState = currentActionControlState;
        currentActionControlState = ActionControlState.None;

        switch (castingCommand)
        {
            case CastingCommand.ContinueChannel:
                // Handle case where channel has ended naturally.
                if (!_channelService.Active)
                {
                    CastingStatus = RemainingAbilityCooldown <= 0f ? CastingStatus.Ready : CastingStatus.Cooldown;
                }
                break;
            case CastingCommand.CancelChannel:
                _channelService.Cancel(ChannelTarget);
                CastingStatus = RemainingAbilityCooldown <= 0f ? CastingStatus.Ready : CastingStatus.Cooldown;

                // Ability whiffed, reset tree. TODO: Make a method out of this including feedback for player. 
                AbilityTree.Reset();
                break;
            case CastingCommand.CastLeft:
                BranchAndCast(Direction.Left);
                break;
            case CastingCommand.CastRight:
                BranchAndCast(Direction.Right);
                break;
        }
    }

    private void BranchAndCast(Direction direction)
    {
        RemainingAbilityCooldown = abilityCooldown;

        if (!AbilityTree.CanWalkDirection(direction))
        {
            // Whiffed!
            AbilityTree.Reset();
            return;
        }

        Ability ability = AbilityTree.Walk(direction);
        Vector3 targetPosition = MouseGamePositionFinder.Instance.GetMouseGamePosition();

        if (ability is InstantCast instantCast)
        {
            Cast(instantCast, targetPosition);

            CastingStatus = CastingStatus.Cooldown;
        }

        if (ability is Channel channel)
        {
            _channelService.Start(channel, targetPosition);

            CastingStatus = direction == Direction.Left
                ? CastingStatus.ChannelingLeft
                : CastingStatus.ChannelingRight;
        }

        if (!AbilityTree.CanWalk())
        {
            AbilityTree.Reset();
        }
    }

    public void Test(Ability ability)
    {
        if (ability is InstantCast instantCast)
        {
            Cast(instantCast, Vector3.back);
        }
    }
}
