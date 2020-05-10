using System;
using UnityEngine;

public class ChannelService : AbilityService
{
    private Channel _currentChannel;

    public Subject ChannelStartSubject { get; } = new Subject();
    public bool Active { get; private set; } = false;
    public float RemainingDuration { get; private set; }
    public float TotalDuration => _currentChannel.Duration;
    public Vector3 TargetPosition { get; set; } = Vector3.zero;

    public ChannelService(Actor actor, Subject lateUpdateSubject, InterruptionManager interruptionManager) : base(actor)
    {
        interruptionManager.Register(InterruptionType.Hard, () => Cancel(TargetPosition));

        lateUpdateSubject.Subscribe(() =>
        {

            if (!Active)
            {
                RemainingDuration = 0f;
                return;
            };

            this.RemainingDuration = Mathf.Max(0f, RemainingDuration - Time.deltaTime);

            if (RemainingDuration > 0f)
            {
                _currentChannel.Continue(TargetPosition);
            }
            else
            {
                _currentChannel.End(TargetPosition);
                Active = false;
            }
        });
    }

    public bool Start(
        AbilityReference abilityReference,
        Vector3 target,
        Action<Subject<bool>> successFeedbackSubjectAction = null
    )
    {
        MovementStatus status = actor.MovementManager.MovementStatus;
        if (status == MovementStatus.Stunned || status == MovementStatus.MovementLocked) return false;

        if (!AbilityLookup.TryGetChannel(abilityReference, actor, GetAbilityDataDiff(abilityReference), out Channel channel)) return false;

        _currentChannel = channel;
        RemainingDuration = _currentChannel.Duration;
        Active = true;
            
        successFeedbackSubjectAction?.Invoke(channel.SuccessFeedbackSubject);
            
        _currentChannel.Start(target);
        _currentChannel.Continue(target);

        ChannelStartSubject.Next();
        return true;
    }

    public void Cancel(Vector3 target)
    {
        if (!Active) return;

        _currentChannel.Cancel(target);
        RemainingDuration = 0f;
        Active = false;
    }
}