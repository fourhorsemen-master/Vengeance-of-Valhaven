using UnityEngine;

public class InstantCastService : AbilityService
{
    public InstantCastService(Player player) : base(player) {}

    public void Cast(
        AbilityReference abilityReference,
        Vector3 floorTargetPosition,
        Vector3 offsetTargetPosition,
        Actor target = null
    )
    {
        AbilityLookup.Instance.TryGetInstantCast(
            abilityReference,
            player,
            GetAbilityDataDiff(abilityReference),
            GetActiveBonuses(abilityReference),
            out InstantCast instantCast
        );

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
            if(animationType != AbilityAnimationType.None && player.AnimController)
            {
                string animationState = AnimationStringLookup.LookupTable[animationType];
                player.AnimController.Play(animationState);
            }
        }
    }
}
