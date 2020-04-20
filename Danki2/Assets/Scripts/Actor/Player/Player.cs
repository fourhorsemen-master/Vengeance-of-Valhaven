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
    private Direction lastCastDirection;
    private bool whiffed = true;
    private Subscription<bool> abilityFeedbackSubscription;

    private Coroutine abilityTimeout = null;

    private ActionControlState previousActionControlState = ActionControlState.None;
    private ActionControlState currentActionControlState = ActionControlState.None;

    [SerializeField]
    private TrailRenderer trailRenderer = null;

    [HideInInspector]
    public float RemainingAbilityCooldown { get; private set; } = 0f;
    [HideInInspector]
    public CastingStatus CastingStatus { get; private set; } = CastingStatus.Ready;

    public AbilityTree AbilityTree { get; private set; }

    public Subject<Tuple<bool, Direction>> AbilityCompletionSubject { get; } = new Subject<Tuple<bool, Direction>>();

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
    }

    protected void Start()
    {
        this.gameObject.tag = Tags.Player;

        AbilityTimeoutSubscription();
    }

    protected override void Update()
    {
        base.Update();

        TickDashCooldown();
        TickAbilityCooldown();
    }

    protected void LateUpdate()
    {
        HandleAbilities();
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

    public void SetCurrentControlState(ActionControlState controlState)
    {
        currentActionControlState = controlState;
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
        whiffed = true;
    }

    private void TickDashCooldown()
    {
        remainingDashCooldown = Mathf.Max(0f, remainingDashCooldown - Time.deltaTime);
    }

    private void TickAbilityCooldown()
    {
        if (ChannelService.Active) return;

        RemainingAbilityCooldown = Mathf.Max(0f, RemainingAbilityCooldown - Time.deltaTime);

        if (RemainingAbilityCooldown > 0f || CastingStatus != CastingStatus.Cooldown) return;

        abilityFeedbackSubscription.Unsubscribe();

        CastingStatus = CastingStatus.Ready;
        AbilityTree.Walk(lastCastDirection);

        if (!AbilityTree.CanWalk() || whiffed)
        {
            AbilityTree.Reset();
        }

        whiffed = true;
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
                if (!ChannelService.Active)
                {
                    CastingStatus = RemainingAbilityCooldown <= 0f ? CastingStatus.Ready : CastingStatus.Cooldown;
                }
                break;
            case CastingCommand.CancelChannel:
                ChannelService.Cancel();
                CastingStatus = RemainingAbilityCooldown <= 0f ? CastingStatus.Ready : CastingStatus.Cooldown;
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
        if (!AbilityTree.CanWalkDirection(direction))
        {
            // Feedback to user that there is no ability here.
            return;
        }

        RemainingAbilityCooldown = abilityCooldown;
        lastCastDirection = direction;
        if (abilityTimeout != null)
        {
            StopCoroutine(abilityTimeout);
        }

        AbilityReference abilityReference = AbilityTree.GetAbility(direction);

        AbilityContext abilityContext = new AbilityContext(
            this,
            MouseGamePositionFinder.Instance.GetMouseGamePosition()
        );

        if (Ability.TryGetInstantCast(
                abilityReference,
                abilityContext,
                out InstantCast instantCast
        ))
        {
            abilityFeedbackSubscription = instantCast.SuccessFeedbackSubject.Subscribe(AbilityFeedbackSubscription);
            instantCast.Cast();
            CastingStatus = CastingStatus.Cooldown;
        }

        if (Ability.TryGetChannel(
                abilityReference,
                abilityContext,
                out Channel channel
        ))
        {
            abilityFeedbackSubscription = channel.SuccessFeedbackSubject.Subscribe(AbilityFeedbackSubscription);
            ChannelService.Start(channel);
            CastingStatus = direction == Direction.Left
                ? CastingStatus.ChannelingLeft
                : CastingStatus.ChannelingRight;
        }
    }

    private void AbilityFeedbackSubscription(bool successful)
    {
        abilityFeedbackSubscription.Unsubscribe();
        whiffed = !successful;
        AbilityCompletionSubject.Next(
            new Tuple<bool, Direction>(successful, lastCastDirection)
        );
    }
}
