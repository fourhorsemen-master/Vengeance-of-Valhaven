﻿using System;
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

    private Vector3 targetPosition = Vector3.zero;

    public float RemainingAbilityCooldown { get; private set; } = 0f;
    public CastingStatus CastingStatus { get; private set; } = CastingStatus.Ready;
    public Subject<Tuple<bool, Direction>> AbilityCompletionSubject { get; } = new Subject<Tuple<bool, Direction>>();

    public AbilityManager(Player player, float abilityTimeoutLimit, float abilityCooldown, Subject updateSubject, Subject lateUpdateSubject)
	{
		this.player = player;
        this.abilityTimeoutLimit = abilityTimeoutLimit;
        this.abilityCooldown = abilityCooldown;

        updateSubject.Subscribe(UpdateTargetPosition);
        updateSubject.Subscribe(TickAbilityCooldown);
        lateUpdateSubject.Subscribe(HandleAbilities);

        AbilityTimeoutSubscription();
    }

    public void SetCurrentControlState(ActionControlState controlState)
    {
        currentActionControlState = controlState;
    }

    private void UpdateTargetPosition()
    {
        // We try to get the mouse game position in scene for the AbilityContext.
        if (!MouseGamePositionFinder.Instance.TryGetMouseGamePosition(out Vector3 mousePosition))
        {
            // If the mouse is outside the scene, we use the mouse position on a horizontal plane at the players height.
            mousePosition = MouseGamePositionFinder.Instance.GetMousePlanePosition(player.transform.position.y, true);
        }

        targetPosition = mousePosition;

        player.ChannelService.TargetPosition = targetPosition;
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
                    player.whiffAudio.Play();
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
                player.ChannelService.Cancel(targetPosition);
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
            // TODO: Feedback to user that there is no ability here.
            return;
        }

        RemainingAbilityCooldown = abilityCooldown;
        lastCastDirection = direction;
        if (abilityTimeout != null)
        {
            player.StopCoroutine(abilityTimeout);
        }

        AbilityReference abilityReference = player.AbilityTree.GetAbility(direction);

        switch (AbilityLookup.GetAbilityType(abilityReference))
        {
            case AbilityType.InstantCast:
                player.InstantCastService.Cast(
                    abilityReference,
                    targetPosition,
                    subject => abilityFeedbackSubscription = subject.Subscribe(AbilityFeedbackSubscription)
                );
                CastingStatus = CastingStatus.Cooldown;
                break;
            case AbilityType.Channel:
                player.ChannelService.Start(
                    abilityReference,
                    targetPosition,
                    subject => abilityFeedbackSubscription = subject.Subscribe(AbilityFeedbackSubscription)
                );
                CastingStatus = direction == Direction.Left
                    ? CastingStatus.ChannelingLeft
                    : CastingStatus.ChannelingRight;
                break;
        }
    }

    private void AbilityFeedbackSubscription(bool successful)
    {
        abilityFeedbackSubscription.Unsubscribe();
        whiffed = !successful;
        if (whiffed) player.whiffAudio.Play();
        AbilityCompletionSubject.Next(
            new Tuple<bool, Direction>(successful, lastCastDirection)
        );
    }
}
