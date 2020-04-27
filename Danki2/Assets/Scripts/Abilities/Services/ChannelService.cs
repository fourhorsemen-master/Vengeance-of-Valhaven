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

    public bool TryGetChannel(AbilityReference abilityReference, out Channel channel)
    {
        return AbilityLookup.TryGetChannel(abilityReference, actor, out channel);
    }

    public void Start(Channel channel, Vector3 target)
    {
        _currentChannel = channel;
        RemainingDuration = _currentChannel.Duration;
        Active = true;

        _currentChannel.Start(target);
        _currentChannel.Continue(target);
    }

    public void Cancel(Vector3 target)
    {
        if (!Active) return;

        _currentChannel.Cancel(target);
        RemainingDuration = 0f;
        Active = false;
    }
}