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
    private Subscription targetDeathSubscription;
    private Enemy target = null;

    public float RemainingCooldownProportion => remainingAbilityCooldown / abilityCooldown;
    public CastingStatus CastingStatus { get; private set; } = CastingStatus.Ready;
    public Subject<Tuple<bool, Direction>> AbilityCompletionSubject { get; } = new Subject<Tuple<bool, Direction>>();

    public AbilityManager(Player player, float abilityTimeoutLimit, float abilityCooldown, Subject updateSubject, Subject lateUpdateSubject)
	{
		this.player = player;
        this.abilityTimeoutLimit = abilityTimeoutLimit;
        this.abilityCooldown = abilityCooldown;

        updateSubject.Subscribe(UpdateTarget);
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

    private void UpdateTarget()
    {
        // First, we raycast for an actor (ie. by ignoring other layers)
        bool mouseHitCollider = MouseGamePositionFinder.Instance.TryGetMouseGamePosition(
            out Vector3 mousePosition,
            out Collider collider,
            Layers.GetLayerMask(new[] { Layers.Actors })
        );

        // Then, if we don't hit any actors, we raycast for any collider
        if (!mouseHitCollider)
        {
            mouseHitCollider = MouseGamePositionFinder.Instance.TryGetMouseGamePosition(out mousePosition, out collider);
        }

        // Then, if no colliders are hit, we use the mouse position on a horizontal plane at the players height.
        if (!mouseHitCollider)
        {
            mousePosition = MouseGamePositionFinder.Instance.GetMousePlanePosition(player.transform.position.y, true);
        }

        SetTargetPosition(mousePosition);

        if (collider != null && collider.gameObject.CompareTag(Tags.Enemy))
        {
            Enemy enemy = collider.gameObject.GetComponent<Enemy>();
            if (!enemy.Dead) SetTarget(enemy);
        }
        else
        {
            RemoveTarget();
        }
    }

    private void SetTargetPosition(Vector3 mousePosition)
    {
        targetPosition = mousePosition;
        player.ChannelService.TargetPosition = targetPosition;
    }

    private void SetTarget(Enemy enemy)
    {
        if (enemy == target) return;

        RemoveTarget();

        enemy.PlayerTargeted.Next(true);
        targetDeathSubscription = enemy.DeathSubject.Subscribe(() => RemoveTarget());
        target = enemy;
        player.ChannelService.Target = enemy;
    }

    private void RemoveTarget()
    {
        if (target == null) return;

        target.PlayerTargeted.Next(false);
        targetDeathSubscription.Unsubscribe();
        target = null;
        player.ChannelService.Target = null;
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
                abilityTimeout = player.WaitAndAct(abilityTimeoutLimit, Whiff);
            }
        });
    }

    private void Whiff()
    {
        if (!player.AbilityTree.AtRoot) player.PlayWhiffSound();

        whiffed = true;
        player.AbilityTree.Reset();
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
                    targetPosition,
                    subject => abilityFeedbackSubscription = subject.Subscribe(AbilityFeedbackSubscription),
                    target
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
