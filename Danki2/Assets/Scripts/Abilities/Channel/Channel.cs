using UnityEngine;

public abstract class Channel : Ability
{
    public float Duration { get; }

    public virtual ChannelType ChannelType => ChannelType.Channel;

    public virtual ChannelEffectOnMovement EffectOnMovement => ChannelEffectOnMovement.Stun;

    protected Channel(Actor owner, AbilityData abilityData, string[] availableBonuses, float duration)
        : base(owner, abilityData, availableBonuses)
    {
        Duration = duration;
    }
    
    public virtual void Start(Vector3 floorTargetPosition, Vector3 offsetTargetPosition) { }

    public virtual void Start(Actor target) => Start(target.transform.position, target.Centre.position);

    public virtual void Continue(Vector3 floorTargetPosition, Vector3 offsetTargetPosition) { }

    public virtual void Continue(Actor target) => Continue(target.transform.position, target.Centre.position);

    public virtual void Cancel(Vector3 floorTargetPosition, Vector3 offsetTargetPosition) { }

    public virtual void Cancel(Actor target) => Cancel(target.transform.position, target.Centre.position);

    public virtual void End(Vector3 floorTargetPosition, Vector3 offsetTargetPosition) { }

    public virtual void End(Actor target) => End(target.transform.position, target.Centre.position);
}
