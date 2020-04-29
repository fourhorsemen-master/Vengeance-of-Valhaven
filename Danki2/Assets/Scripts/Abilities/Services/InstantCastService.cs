using System;
using UnityEngine;

public class InstantCastService
{
    private readonly Actor actor;
    
    public InstantCastService(Actor actor)
    {
        this.actor = actor;
    }

    public bool Cast(
        AbilityReference abilityReference,
        Vector3 target,
        Action<Subject<bool>> successFeedbackSubjectAction = null
    )
    {
        MovementStatus status = actor.MovementManager.MovementStatus;
        if (status == MovementStatus.Stunned || status == MovementStatus.MovementLocked) return false;

        if (!AbilityLookup.TryGetInstantCast(abilityReference, actor, AbilityData.Zero, out InstantCast instantCast)) return false;

        successFeedbackSubjectAction?.Invoke(instantCast.SuccessFeedbackSubject);
        instantCast.Cast(target);
        return true;
    }
}
