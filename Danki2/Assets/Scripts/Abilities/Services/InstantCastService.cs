using UnityEngine;

public class InstantCastService : AbilityService
{
    public InstantCastService(Actor actor) : base(actor) {}

    public bool TryCast(
        AbilityReference abilityReference,
        Vector3 floorTargetPosition,
        Vector3 offsetTargetPosition,
        Actor target = null
    )
    {
        if (!actor.CanCast) return false;

        if (!AbilityLookup.Instance.TryGetInstantCast(
            abilityReference,
            actor,
            GetAbilityDataDiff(abilityReference),
            GetActiveBonuses(abilityReference),
            out InstantCast instantCast
        )) return false;

        SubscribeToFeedback(instantCast);
        StartFeedbackTimer();

        if (target != null)
        {
            instantCast.Cast(target);
        }
        else
        {
            instantCast.Cast(floorTargetPosition, offsetTargetPosition);
        }

        if(AbilityLookup.Instance.TryGetAnimationType(abilityReference, out AbilityAnimationType animationType))
        {
            if(animationType != AbilityAnimationType.None && actor.AnimController)
            {
                string animationState = AnimationStringLookup.LookupTable[animationType];
                actor.AnimController.Play(animationState);
            }
        }

        return true;
    }
}
