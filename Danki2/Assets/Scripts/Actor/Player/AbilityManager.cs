using System;
using UnityEngine;

public class AbilityManager
{
	private readonly Player player;
    private readonly float comboTimeout;
    private readonly float feedbackTimeout;
    private readonly float cooldownDuringCombo;
    private readonly float cooldownAfterCombo;
    private float currentCooldownPeriod = 1f;
    private float remainingAbilityCooldown = 0f;

    private Direction lastCastDirection;
    private bool whiffed = true;
    private bool feedbackRecieved = false;
    private Coroutine abilityTimeout = null;
    private Coroutine feedbackTimer = null;
    private Subscription<bool> abilityFeedbackSubscription;
    private ActionControlState previousActionControlState = ActionControlState.None;

    public float RemainingCooldownProportion => remainingAbilityCooldown / currentCooldownPeriod;
    public CastingStatus CastingStatus { get; private set; } = CastingStatus.Ready;
    public Subject<Tuple<bool, Direction>> AbilityCompletionSubject { get; } = new Subject<Tuple<bool, Direction>>();

    public AbilityManager(Player player, Subject updateSubject, Subject lateUpdateSubject)
	{
		this.player = player;
        comboTimeout = player.comboTimeout;
        feedbackTimeout = player.feedbackTimeout;
        cooldownDuringCombo = player.cooldownDuringCombo;
        cooldownAfterCombo = player.cooldownAfterCombo;

        if (player.rollResetsCombo)
        {
            this.player.RollSubject.Subscribe(() => {
                if (!player.AbilityTree.AtRoot) Whiff();
            });
        }

        updateSubject.Subscribe(TickAbilityCooldown);
        lateUpdateSubject.Subscribe(HandleAbilities);

        this.player.HealthManager.ModifiedDamageSubject.Subscribe(d =>
        {
            if (d.Damage > 0) Whiff();
        });

        AbilityTimeoutSubscription();
    }

    private void TickAbilityCooldown()
    {
        if (CastingStatus != CastingStatus.Cooldown) return;

        remainingAbilityCooldown = Mathf.Max(0f, remainingAbilityCooldown - Time.deltaTime);

        if (remainingAbilityCooldown > 0f) return;

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
        feedbackRecieved = false;
    }

    private void StartFeedbackTimer()
    {
        StopFeedbackTimer();

        feedbackTimer = player.WaitAndAct(feedbackTimeout, Whiff);
    }

    private void StopFeedbackTimer()
    {
        if (feedbackTimer != null)
        {
            player.StopCoroutine(feedbackTimer);
        }
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
                abilityTimeout = player.WaitAndAct(comboTimeout, Whiff);
            }
        });
    }

    private void Whiff()
    {
        player.PlayWhiffSound();

        whiffed = true;
        player.AbilityTree.Reset();

        ResetCooldown(cooldownAfterCombo);

        CastingStatus = CastingStatus.Cooldown;
    }

    private void HandleAbilities()
    {
        ActionControlState currentActionControlState = PlayerControls.Instance.ActionControlState;

        CastingCommand castingCommand = ControlMatrix.GetCastingCommand(
            CastingStatus,
            previousActionControlState,
            currentActionControlState
        );

        previousActionControlState = currentActionControlState;

        switch (castingCommand)
        {
            case CastingCommand.ContinueChannel:
                // Handle case where channel has ended naturally.
                if (!player.ChannelService.Active)
                {
                    if (feedbackRecieved)
                    {
                        CastingStatus = CastingStatus.Cooldown;
                    }
                    else
                    {
                        CastingStatus = CastingStatus.AwaitingFeedback;
                        StartFeedbackTimer();
                    }
                }
                break;
            case CastingCommand.CancelChannel:
                player.ChannelService.CancelChannel();
                if (feedbackRecieved)
                {
                    CastingStatus = CastingStatus.Cooldown;
                }
                else
                {
                    CastingStatus = CastingStatus.AwaitingFeedback;
                    StartFeedbackTimer();
                }
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

        if (abilityTimeout != null)
        {
            player.StopCoroutine(abilityTimeout);
        }

        switch (AbilityLookup.Instance.GetAbilityType(abilityReference))
        {
            case AbilityType.InstantCast:
                CastingStatus = CastingStatus.AwaitingFeedback;
                player.InstantCastService.Cast(
                    abilityReference,
                    player.TargetFinder.TargetPosition,
                    subject => abilityFeedbackSubscription = subject.Subscribe(AbilityFeedbackSubscription),
                    player.TargetFinder.Target
                );
                break;
            case AbilityType.Channel:
                CastingStatus = direction == Direction.Left
                    ? CastingStatus.ChannelingLeft
                    : CastingStatus.ChannelingRight;
                player.ChannelService.StartChannel(
                    abilityReference,
                    subject => abilityFeedbackSubscription = subject.Subscribe(AbilityFeedbackSubscription)
                );
                break;
        }

        if (CastingStatus == CastingStatus.AwaitingFeedback)
        {
            StartFeedbackTimer();
        }
    }

    private void AbilityFeedbackSubscription(bool successful)
    {
        abilityFeedbackSubscription.Unsubscribe();

        StopFeedbackTimer();

        feedbackRecieved = true;

        whiffed = !successful;

        if (whiffed) player.PlayWhiffSound();

        if (CastingStatus == CastingStatus.AwaitingFeedback)
        {
            CastingStatus = CastingStatus.Cooldown;
        }

        float cooldownTime = whiffed || ComboComplete()
            ? cooldownAfterCombo
            : cooldownDuringCombo;

        ResetCooldown(cooldownTime);

        AbilityCompletionSubject.Next(
            new Tuple<bool, Direction>(successful, lastCastDirection)
        );
    }

    private bool ComboComplete()
    {
        return player.AbilityTree.WalkingEndsCombo(lastCastDirection);
    }

    private void ResetCooldown(float duration)
    {
        currentCooldownPeriod = duration;
        remainingAbilityCooldown = duration;
    }
}
