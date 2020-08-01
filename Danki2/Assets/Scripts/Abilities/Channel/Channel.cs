using UnityEngine;

public abstract class Channel : Ability
{
    public abstract float Duration { get; }

    public virtual ChannelType ChannelType => ChannelType.Channel;

    public virtual ChannelEffectOnMovement EffectOnMovement => ChannelEffectOnMovement.Stun;

    protected Channel(Actor owner, AbilityData abilityData, string[] availableBonuses) : base(owner, abilityData, availableBonuses)
    {
    }
    
    public virtual void Start(Vector3 target) { }

    public virtual void Start(Actor target) => Start(target.Centre);

    public virtual void Continue(Vector3 target) { }

    public virtual void Continue(Actor target) => Continue(target.Centre);

    public virtual void Cancel(Vector3 target) { }

    public virtual void Cancel(Actor target) => Cancel(target.Centre);

    public virtual void End(Vector3 target) { }

    public virtual void End(Actor target) => End(target.Centre);
}
