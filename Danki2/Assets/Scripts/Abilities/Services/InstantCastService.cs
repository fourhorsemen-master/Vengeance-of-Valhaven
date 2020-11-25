using System;
using UnityEngine;

public class InstantCastService : AbilityService
{
    public InstantCastService(Actor actor) : base(actor) {}

    public bool TryCast(
        AbilityReference abilityReference,
        Vector3 floorTargetPosition,
        Vector3 offsetTargetPosition,
        Action<Subject<bool>> successFeedbackSubjectAction = null,
        Actor target = null
    )
    {
        if (!CanCast) return false;

        if (!AbilityLookup.Instance.TryGetInstantCast(
            abilityReference,
            actor,
            GetAbilityDataDiff(abilityReference),
            GetActiveBonuses(abilityReference),
            out InstantCast instantCast
        )) return false;

        successFeedbackSubjectAction?.Invoke(instantCast.SuccessFeedbackSubject);

        if (target != null)
        {
            instantCast.Cast(target);
        }
        else
        {
            instantCast.Cast(floorTargetPosition, offsetTargetPosition);
        }

        return true;
    }
}
