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
        Vector3 target,
        Action<Subject<bool>> successFeedbackSubjectAction = null
    )
    {
        MovementStatus status = actor.MovementManager.MovementStatus;
        if (status == MovementStatus.Stunned || status == MovementStatus.MovementLocked) return false;

        if (!AbilityLookup.TryGetInstantCast(abilityReference, actor, GetAbilityDataDiff(abilityReference), out InstantCast instantCast)) return false;

        successFeedbackSubjectAction?.Invoke(instantCast.SuccessFeedbackSubject);
        instantCast.Cast(target);
        CastSubject.Next();
        return true;
    }
}
