﻿using System;
using UnityEngine;

public class AbilityManager
{
	private readonly Player player;
    private readonly float abilityTimeoutLimit;
    private readonly float abilityCooldown;
    private float remainingAbilityCooldown = 0f;

    private Direction lastCastDirection;
    private bool whiffed = true;
    private Coroutine abilityTimeout = null;
    private Subscription<bool> abilityFeedbackSubscription;
    private ActionControlState previousActionControlState = ActionControlState.None;
    private ActionControlState currentActionControlState = ActionControlState.None;
    private Vector3 targetPosition = Vector3.zero;

    public float RemainingCooldownProportion => remainingAbilityCooldown / abilityCooldown;
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
        remainingAbilityCooldown = Mathf.Max(0f, remainingAbilityCooldown - decrement);

        if (remainingAbilityCooldown > 0f || CastingStatus != CastingStatus.Cooldown) return;

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
                    player.PlayWhiffSound();
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
                    CastingStatus = remainingAbilityCooldown <= 0f ? CastingStatus.Ready : CastingStatus.Cooldown;
                }
                break;
            case CastingCommand.CancelChannel:
                player.ChannelService.Cancel(targetPosition);
                CastingStatus = remainingAbilityCooldown <= 0f ? CastingStatus.Ready : CastingStatus.Cooldown;
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
        if (!player.AbilityTree.CanWalkDirection(direction)) return;
        lastCastDirection = direction;

        AbilityReference abilityReference = player.AbilityTree.GetAbility(direction);
        bool abilityCast = false;
        CastingStatus nextStatus = CastingStatus;

        switch (AbilityLookup.Instance.GetAbilityType(abilityReference))
        {
            case AbilityType.InstantCast:
                abilityCast = player.InstantCastService.Cast(
                    abilityReference,
                    targetPosition,
                    subject => abilityFeedbackSubscription = subject.Subscribe(AbilityFeedbackSubscription)
                );
                nextStatus = CastingStatus.Cooldown;
                break;
            case AbilityType.Channel:
                abilityCast = player.ChannelService.Start(
                    abilityReference,
                    targetPosition,
                    subject => abilityFeedbackSubscription = subject.Subscribe(AbilityFeedbackSubscription)
                );
                nextStatus = direction == Direction.Left
                    ? CastingStatus.ChannelingLeft
                    : CastingStatus.ChannelingRight;
                break;
        }

        if (!abilityCast)
        {
            player.PlayWhiffSound();
            return;
        }

        CastingStatus = nextStatus;
        remainingAbilityCooldown = abilityCooldown;
        if (abilityTimeout != null)
        {
            player.StopCoroutine(abilityTimeout);
        }
    }

    private void AbilityFeedbackSubscription(bool successful)
    {
        abilityFeedbackSubscription.Unsubscribe();
        whiffed = !successful;
        if (whiffed) player.PlayWhiffSound();
        AbilityCompletionSubject.Next(
            new Tuple<bool, Direction>(successful, lastCastDirection)
        );
    }
}
