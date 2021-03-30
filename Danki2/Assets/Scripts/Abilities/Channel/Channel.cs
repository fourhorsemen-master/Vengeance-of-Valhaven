using UnityEngine;

public abstract class Channel : Ability
{
    public float Duration { get; }

    public virtual ChannelType ChannelType => ChannelType.Channel;

    public virtual ChannelEffectOnMovement EffectOnMovement => ChannelEffectOnMovement.Stun;

    protected Channel(AbilityConstructionArgs arguments)
        : base(arguments)
    {
        Duration = arguments.ChannelDuration;
    }
    
    public virtual void Start(Vector3 floorTargetPosition, Vector3 offsetTargetPosition) { }

    public virtual void Start(Actor target) => Start(target.transform.position, target.Centre);

    public virtual void Continue(Vector3 floorTargetPosition, Vector3 offsetTargetPosition) { }

    public virtual void Continue(Actor target) => Continue(target.transform.position, target.Centre);

    public virtual void Cancel(Vector3 floorTargetPosition, Vector3 offsetTargetPosition) { }

    public virtual void Cancel(Actor target) => Cancel(target.transform.position, target.Centre);

    public virtual void End(Vector3 floorTargetPosition, Vector3 offsetTargetPosition) { }

    public virtual void End(Actor target) => End(target.transform.position, target.Centre);
}
