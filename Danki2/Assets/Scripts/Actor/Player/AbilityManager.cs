using System;
using UnityEngine;

public class AbilityManager
{
	private readonly Player player;
    private readonly float abilityTimeoutLimit;
    private readonly float abilityCooldown;

    private Direction lastCastDirection;
    private bool whiffed = true;
    private Coroutine abilityTimeout = null;
    private Subscription<bool> abilityFeedbackSubscription;
    private ActionControlState previousActionControlState = ActionControlState.None;
    private ActionControlState currentActionControlState = ActionControlState.None;

    public float RemainingAbilityCooldown { get; private set; } = 0f;
    public CastingStatus CastingStatus { get; private set; } = CastingStatus.Ready;
    public Subject<Tuple<bool, Direction>> AbilityCompletionSubject { get; } = new Subject<Tuple<bool, Direction>>();

    public AbilityManager(Player player, float abilityTimeoutLimit, float abilityCooldown, Subject updateSubject, Subject lateUpdateSubject)
	{
		this.player = player;
        this.abilityTimeoutLimit = abilityTimeoutLimit;
        this.abilityCooldown = abilityCooldown;

        updateSubject.Subscribe(TickAbilityCooldown);
        lateUpdateSubject.Subscribe(HandleAbilities);

        AbilityTimeoutSubscription();
    }

    public void SetCurrentControlState(ActionControlState controlState)
    {
        currentActionControlState = controlState;
    }
    
    private void TickAbilityCooldown()
    {
        if (player.ChannelService.Active) return;

        float decrement = whiffed ? Time.deltaTime / 2 : Time.deltaTime;
        RemainingAbilityCooldown = Mathf.Max(0f, RemainingAbilityCooldown - decrement);

        if (RemainingAbilityCooldown > 0f || CastingStatus != CastingStatus.Cooldown) return;

        abilityFeedbackSubscription.Unsubscribe();

        CastingStatus = CastingStatus.Ready;
        player.AbilityTree.Walk(lastCastDirection);

        if (!player.AbilityTree.CanWalk() || whiffed)
        {
            player.AbilityTree.Reset();
        }

        whiffed = true;
    }

    private void AbilityTimeoutSubscription()
    {
        player.AbilityTree.CurrentDepthSubject.Subscribe((int treeDepth) =>
        {
            if (abilityTimeout != null)
            {
                player.StopCoroutine(abilityTimeout);
            }

            if (treeDepth > 0)
            {
                abilityTimeout = player.WaitAndAct(abilityTimeoutLimit, () =>
                {
                    player.AbilityTree.Reset();
                    whiffed = true;
                });
            }
        });
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
                if (!player.ChannelService.Active)
                {
                    CastingStatus = RemainingAbilityCooldown <= 0f ? CastingStatus.Ready : CastingStatus.Cooldown;
                }
                break;
            case CastingCommand.CancelChannel:
                player.ChannelService.Cancel();
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
        if (!player.AbilityTree.CanWalkDirection(direction))
        {
            // Feedback to user that there is no ability here.
            return;
        }

        RemainingAbilityCooldown = abilityCooldown;
        lastCastDirection = direction;
        if (abilityTimeout != null)
        {
            player.StopCoroutine(abilityTimeout);
        }

        AbilityReference abilityReference = player.AbilityTree.GetAbility(direction);

        AbilityContext abilityContext = new AbilityContext(
            player,
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
            player.ChannelService.Start(channel);
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
