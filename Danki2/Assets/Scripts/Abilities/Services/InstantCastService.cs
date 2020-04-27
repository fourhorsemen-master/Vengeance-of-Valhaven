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
        MovementStatus status = actor.MovementManager.MovementStatus;
        if (status == MovementStatus.Stunned || status == MovementStatus.MovementLocked) return;

        if (AbilityLookup.TryGetInstantCast(abilityReference, actor, out InstantCast instantCast))
        {
            successFeedbackSubjectAction?.Invoke(instantCast.SuccessFeedbackSubject);
            instantCast.Cast(target);
        }
    }
}
