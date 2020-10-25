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

    public float RemainingCooldownProportion => remainingAbilityCooldown / abilityCooldown;
    public CastingStatus CastingStatus { get; private set; } = CastingStatus.Ready;
    public Subject<Tuple<bool, Direction>> AbilityCompletionSubject { get; } = new Subject<Tuple<bool, Direction>>();

    public AbilityManager(Player player, Subject updateSubject, Subject lateUpdateSubject)
	{
		this.player = player;
        this.abilityTimeoutLimit = player.Settings.ComboTimeout;
        this.abilityCooldown = player.Settings.CooldownDuringCombo;

        updateSubject.Subscribe(TickAbilityCooldown);
        lateUpdateSubject.Subscribe(HandleAbilities);

        this.player.RollSubject.Subscribe(Whiff);
        this.player.HealthManager.ModifiedDamageSubject.Subscribe(d =>
        {
            if (d.Damage > 0) Whiff();
        });

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
        remainingAbilityCooldown = Mathf.Max(0f, remainingAbilityCooldown - decrement);

        if (remainingAbilityCooldown > 0f || CastingStatus != CastingStatus.Cooldown) return;

        if (abilityFeedbackSubscription != null)
        {
            abilityFeedbackSubscription.Unsubscribe();
        }

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
                abilityTimeout = player.WaitAndAct(abilityTimeoutLimit, Whiff);
            }
        });
    }

    private void Whiff()
    {
        if (!player.AbilityTree.AtRoot) player.PlayWhiffSound();

        whiffed = true;
        player.AbilityTree.Reset();
        remainingAbilityCooldown = abilityCooldown;
        CastingStatus = CastingStatus.Cooldown;
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
                player.ChannelService.CancelChannel();
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
                    player.TargetFinder.TargetPosition,
                    subject => abilityFeedbackSubscription = subject.Subscribe(AbilityFeedbackSubscription),
                    player.TargetFinder.Target
                );
                nextStatus = CastingStatus.Cooldown;
                break;
            case AbilityType.Channel:
                abilityCast = player.ChannelService.StartChannel(
                    abilityReference,
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
