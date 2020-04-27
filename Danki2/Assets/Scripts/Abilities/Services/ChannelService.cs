using System;
using UnityEngine;

public class ChannelService
{
    private readonly Actor actor;
    
    private Channel _currentChannel;

    public bool Active { get; private set; } = false;
    public float RemainingDuration { get; private set; }
    public float TotalDuration => _currentChannel.Duration;
    public Vector3 TargetPosition { get; set; } = Vector3.zero;

    public ChannelService(Actor actor, Subject lateUpdateSubject, InterruptionManager interruptionManager)
    {
        this.actor = actor;
        
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

    public Subscription<bool> Start(AbilityReference abilityReference, Vector3 target, Action<bool> abilityFeedbackSubscription = null)
    {
        if (AbilityLookup.TryGetChannel(abilityReference, actor, out Channel channel))
        {
            _currentChannel = channel;
            RemainingDuration = _currentChannel.Duration;
            Active = true;
            Subscription<bool> subscription = abilityFeedbackSubscription != null
                ? channel.SuccessFeedbackSubject.Subscribe(abilityFeedbackSubscription)
                : null;
            _currentChannel.Start(target);
            _currentChannel.Continue(target);

            return subscription;
        }

        return null;
    }

    public void Cancel(Vector3 target)
    {
        if (!Active) return;

        _currentChannel.Cancel(target);
        RemainingDuration = 0f;
        Active = false;
    }
}