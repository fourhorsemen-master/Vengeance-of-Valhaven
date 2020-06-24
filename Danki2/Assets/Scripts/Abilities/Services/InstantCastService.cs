using System;
using UnityEngine;

public class InstantCastService : AbilityService
{
    public Subject CastSubject { get; } = new Subject();

    public InstantCastService(Actor actor) : base(actor)
    {
    }

    public bool Cast(
        AbilityReference abilityReference,
        Vector3 targetPosition,
        Action<Subject<bool>> successFeedbackSubjectAction = null,
        Actor target = null
    )
    {
        MovementStatus status = actor.MovementManager.MovementStatus;
        if (status == MovementStatus.Stunned || status == MovementStatus.MovementLocked) return false;

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
            instantCast.Cast(targetPosition);
        }

        CastSubject.Next();
        return true;
    }
}
