﻿using System;
using UnityEngine;

public class ChannelService : AbilityService, MovementStatusProvider
{
    private Channel _currentChannel;

    public Subject<ChannelType> ChannelStartSubject { get; } = new Subject<ChannelType>();
    public bool Active { get; private set; } = false;
    public float RemainingDuration { get; private set; }
    public float TotalDuration => _currentChannel.Duration;
    public Vector3 TargetPosition { get; set; } = Vector3.zero;
    public Actor Target { get; set; } = null;
    public bool HasTarget => Target != null;

    public ChannelService(Actor actor, Subject lateUpdateSubject, InterruptionManager interruptionManager) : base(actor)
    {
        interruptionManager.Register(InterruptionType.Soft, CancelChannel);

        actor.DeathSubject.Subscribe(CancelChannel);

        lateUpdateSubject.Subscribe(() =>
        {
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
        });
    }

    public bool SetStunned() => Active && _currentChannel.EffectOnMovement == ChannelEffectOnMovement.Stun;

    public bool SetRooted() => Active && _currentChannel.EffectOnMovement == ChannelEffectOnMovement.Root;

    public bool StartChannel(
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
            _currentChannel.Start(TargetPosition);
            _currentChannel.Continue(TargetPosition);
        }

        ChannelStartSubject.Next(channel.ChannelType);
        return true;
    }

    public void CancelChannel()
    {
        if (!Active) return;

        RemainingDuration = 0f;
        Active = false;

        if (HasTarget)
        {
            _currentChannel.Cancel(Target);
        }
        else
        {
            _currentChannel.Cancel(TargetPosition);
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
            _currentChannel.Continue(TargetPosition);
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
            _currentChannel.End(TargetPosition);
        }
    }
}