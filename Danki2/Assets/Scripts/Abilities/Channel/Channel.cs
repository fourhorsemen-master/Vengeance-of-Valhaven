﻿using UnityEngine;

public abstract class Channel : Ability
{
    public abstract float Duration { get; }

    public virtual ChannelType ChannelType => ChannelType.Channel;

    protected Channel(Actor owner, AbilityData abilityData, string[] availableBonuses) : base(owner, abilityData, availableBonuses)
    {
    }
    
    public virtual void Start(Vector3 target) { }

    public virtual void Start(Actor actor) => Start(actor.transform.position);

    public virtual void Continue(Vector3 target) { }

    public virtual void Continue(Actor actor) => Start(actor.transform.position);

    public virtual void Cancel(Vector3 target) { }

    public virtual void Cancel(Actor actor) => Start(actor.transform.position);

    public virtual void End(Vector3 target) { }

    public virtual void End(Actor actor) => Start(actor.transform.position);
}
