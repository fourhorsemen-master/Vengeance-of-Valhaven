using System;
using UnityEngine;

public class InstantCastService
{
    private readonly Actor actor;
    
    public InstantCastService(Actor actor)
    {
        this.actor = actor;
    }

    public void Cast(
        AbilityReference abilityReference,
        Vector3 target,
        Action<Subject<bool>> successFeedbackSubjectAction = null
    )
    {
        if (AbilityLookup.TryGetInstantCast(abilityReference, actor, out InstantCast instantCast))
        {
            successFeedbackSubjectAction?.Invoke(instantCast.SuccessFeedbackSubject);
            instantCast.Cast(target);
        }
    }
}
