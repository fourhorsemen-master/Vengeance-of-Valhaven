﻿using UnityEngine;

public abstract class Cast : Channel
{
    public sealed override ChannelType ChannelType => ChannelType.Cast;
    public sealed override float Duration => CastTime;

    protected abstract float CastTime { get; }

    protected Cast(Actor owner, AbilityData abilityData, string[] availableBonuses)
        : base(owner, abilityData, availableBonuses)
    {
    }

    public sealed override void Start(Vector3 target) { }

    public sealed override void Start(Actor actor) { }

    public sealed override void Continue(Vector3 target) { }

    public sealed override void Continue(Actor actor) { }

    public sealed override void Cancel(Vector3 target) => SuccessFeedbackSubject.Next(false);

    public sealed override void Cancel(Actor actor) => SuccessFeedbackSubject.Next(false);
}