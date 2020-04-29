using System;
using System.Collections.Generic;
using UnityEngine;

public class InstantCastService
{
    private readonly Actor actor;

    private readonly List<IAbilityDataDiffer> differs = new List<IAbilityDataDiffer>();
    
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

        if (!AbilityLookup.TryGetInstantCast(abilityReference, actor, GetAbilityDataDiff(), out InstantCast instantCast)) return false;

        successFeedbackSubjectAction?.Invoke(instantCast.SuccessFeedbackSubject);
        instantCast.Cast(target);
        return true;
    }

    public void RegisterAbilityDataDiffer(IAbilityDataDiffer differ)
    {
        this.differs.Add(differ);
    }

    private AbilityData GetAbilityDataDiff()
    {
        AbilityData abilityData = AbilityData.Zero;
        differs.ForEach(c => abilityData += c.GetAbilityDataDiff());
        return abilityData;
    }
}
