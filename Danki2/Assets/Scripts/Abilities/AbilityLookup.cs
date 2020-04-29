using System;
using System.Collections.Generic;
using UnityEngine;

public static class AbilityLookup
{
    private static Dictionary<AbilityReference, Func<Actor, AbilityData, InstantCast>> instantCasts 
        = new Dictionary<AbilityReference, Func<Actor, AbilityData, InstantCast>>
        {
            { AbilityReference.Slash, (a, b) => new Slash(a, b) },
            { AbilityReference.Fireball, (a, b) => new Fireball(a, b) },
            { AbilityReference.DaggerThrow, (a, b) => new DaggerThrow(a, b) },
            { AbilityReference.Bite, (a, b) => new Bite(a, b) },
            { AbilityReference.Roll, (a, b) => new Roll(a, b) },
            { AbilityReference.Lunge, (a, b) => new Lunge(a, b) },
            { AbilityReference.Pounce, (a, b) => new Pounce(a, b) },
            { AbilityReference.Smash, (a, b) => new Smash(a, b) },
        };

    private static Dictionary<AbilityReference, Func<Actor, AbilityData, Channel>> channels 
        = new Dictionary<AbilityReference, Func<Actor, AbilityData, Channel>>
        {
            { AbilityReference.Whirlwind, (a, b) => new Whirlwind(a, b) },
        };

    private static Dictionary<AbilityReference, AbilityData> abilityDataLookup
        = new Dictionary<AbilityReference, AbilityData>
        {
            { AbilityReference.Slash, Slash.BaseAbilityData },
            { AbilityReference.Fireball, Fireball.BaseAbilityData },
            { AbilityReference.DaggerThrow, DaggerThrow.BaseAbilityData },
            { AbilityReference.Bite, Bite.BaseAbilityData },
            { AbilityReference.Roll, Roll.BaseAbilityData },
            { AbilityReference.Lunge, Lunge.BaseAbilityData },
            { AbilityReference.Pounce, Pounce.BaseAbilityData },
            { AbilityReference.Smash, Smash.BaseAbilityData },
            { AbilityReference.Whirlwind, Whirlwind.BaseAbilityData },
        };

    private static readonly Dictionary<AbilityReference, AbilityType> abilityTypes = new Dictionary<AbilityReference, AbilityType>();

    static AbilityLookup()
    {
        foreach (AbilityReference abilityReference in Enum.GetValues(typeof(AbilityReference)))
        {
            bool isInInstantCasts = instantCasts.ContainsKey(abilityReference);
            bool isInChannels = channels.ContainsKey(abilityReference);

            if (isInInstantCasts && isInChannels)
            {
                Debug.LogError($"{abilityReference.ToString()} is in both ability lookups");
            }

            if (!isInInstantCasts && !isInChannels)
            {
                Debug.LogError($"{abilityReference.ToString()} is in no ability lookup");
            }

            abilityTypes[abilityReference] = isInInstantCasts ? AbilityType.InstantCast : AbilityType.Channel;
        }
    }

    public static bool TryGetInstantCast(
        AbilityReference abilityReference,
        Actor owner,
        AbilityData abilityDataDiff,
        out InstantCast ability
    )
    {
        if (instantCasts.ContainsKey(abilityReference))
        {
            AbilityData abilityData = abilityDataLookup[abilityReference] + abilityDataDiff;
            ability = instantCasts[abilityReference](owner, abilityData);
            return true;
        }

        ability = null;
        return false;
    }

    public static bool TryGetChannel(
        AbilityReference abilityReference,
        Actor owner,
        AbilityData abilityDataDiff,
        out Channel ability
    )
    {
        if (channels.ContainsKey(abilityReference))
        {
            AbilityData abilityData = abilityDataLookup[abilityReference] + abilityDataDiff;
            ability = channels[abilityReference](owner, abilityData);
            return true;
        }

        ability = null;
        return false;
    }

    public static AbilityType GetAbilityType(AbilityReference abilityReference)
    {
        return abilityTypes[abilityReference];
    }
}
