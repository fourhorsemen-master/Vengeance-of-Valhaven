using System;
using UnityEngine;

public class InstantCastService
{
    private readonly Actor actor;
    
    public InstantCastService(Actor actor)
    {
        this.actor = actor;
    }
    
    public Subscription<bool> Cast(AbilityReference abilityReference, Vector3 target, Action<bool> abilityFeedbackSubscription = null)
    {
        if (AbilityLookup.TryGetInstantCast(abilityReference, actor, out InstantCast instantCast))
        {
            Subscription<bool> subscription = abilityFeedbackSubscription != null
                ? instantCast.SuccessFeedbackSubject.Subscribe(abilityFeedbackSubscription)
                : null;
            instantCast.Cast(target);
            return subscription;
        }

        return null;
    }
}
