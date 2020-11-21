﻿using System;
using UnityEngine;

public class ChannelService : AbilityService, IMovementStatusProvider
{
    private Channel _currentChannel;

    public Subject<ChannelType> ChannelStartSubject { get; } = new Subject<ChannelType>();
    public Subject ChannelEndSubject { get; } = new Subject();
    public bool Active { get; private set; } = false;
    public float RemainingDuration { get; private set; }
    public float TotalDuration => _currentChannel.Duration;
    public Vector3 FloorTargetPosition { get; set; } = Vector3.zero;
    public Vector3 OffsetTargetPosition { get; set; } = Vector3.zero;
    public Actor Target { get; set; } = null;

    private bool HasTarget => Target != null;

    public ChannelService(Actor actor, Subject startSubject, Subject lateUpdateSubject) : base(actor)
    {
        startSubject.Subscribe(Setup);
        lateUpdateSubject.Subscribe(TickChannel);
    }

    public bool Stuns() => Active && _currentChannel.EffectOnMovement == ChannelEffectOnMovement.Stun;

    public bool Roots() => Active && _currentChannel.EffectOnMovement == ChannelEffectOnMovement.Root;

    private void Setup()
    {
        actor.InterruptionManager.Register(
            InterruptionType.Soft,
            CancelChannel,
            InterruptibleFeature.InterruptOnDeath,
            InterruptibleFeature.Repeat
        );
    }

    public bool TryStartChannel(
        AbilityReference abilityReference,
        Action<Subject<bool>> successFeedbackSubjectAction = null
    )
    {
        if (!CanCast) return false;

        if (!AbilityLookup.Instance.TryGetChannel(
            abilityReference,
            actor,
            GetAbilityDataDiff(abilityReference),
            GetActiveBonuses(abilityReference),
            out Channel channel
        )) return false;

        _currentChannel = channel;
        RemainingDuration = _currentChannel.Duration;
        Active = true;
            
        successFeedbackSubjectAction?.Invoke(channel.SuccessFeedbackSubject);

        if (HasTarget)
        {
            _currentChannel.Start(Target);
            _currentChannel.Continue(Target);
        }
        else
        {
            _currentChannel.Start(FloorTargetPosition, OffsetTargetPosition);
            _currentChannel.Continue(FloorTargetPosition, OffsetTargetPosition);
        }

        ChannelStartSubject.Next(channel.ChannelType);
        return true;
    }

    public void CancelChannel()
    {
        if (!Active || actor.Dead) return;

        RemainingDuration = 0f;
        Active = false;

        if (HasTarget)
        {
            _currentChannel.Cancel(Target);
        }
        else
        {
            _currentChannel.Cancel(FloorTargetPosition, OffsetTargetPosition);
        }

        ChannelEndSubject.Next();
    }

    private void TickChannel()
    {
        if (actor.Dead) return;

        if (!Active)
        {
            RemainingDuration = 0f;
            return;
        };

        RemainingDuration = Mathf.Max(0f, RemainingDuration - Time.deltaTime);

        if (RemainingDuration > 0f)
        {
            ContinueChannel();
        }
        else
        {
            EndChannel();
        }
    }

    private void ContinueChannel()
    {
        if (HasTarget)
        {
            _currentChannel.Continue(Target);
        }
        else
        {
            _currentChannel.Continue(FloorTargetPosition, OffsetTargetPosition);
        }
    }

    private void EndChannel()
    {
        Active = false;

        if (HasTarget)
        {
            _currentChannel.End(Target);
        }
        else
        {
            _currentChannel.End(FloorTargetPosition, OffsetTargetPosition);
        }

        ChannelEndSubject.Next();
    }
}
