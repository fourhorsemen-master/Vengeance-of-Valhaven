using System;
using System.Collections.Generic;
using UnityEngine;

public static class AbilityLookup
{
    private static Dictionary<AbilityReference, Func<Actor, InstantCast>> instantCasts 
        = new Dictionary<AbilityReference, Func<Actor, InstantCast>>
        {
            { AbilityReference.Slash, (a) => new Slash(a, abilityData[AbilityReference.Slash]) },
            { AbilityReference.Fireball, (a) => new Fireball(a, abilityData[AbilityReference.Fireball]) },
            { AbilityReference.DaggerThrow, (a) => new DaggerThrow(a, abilityData[AbilityReference.DaggerThrow]) },
            { AbilityReference.Bite, (a) => new Bite(a, abilityData[AbilityReference.Bite]) },
            { AbilityReference.Roll, (a) => new Roll(a, abilityData[AbilityReference.Roll]) },
            { AbilityReference.Lunge, (a) => new Lunge(a, abilityData[AbilityReference.Lunge]) },
            { AbilityReference.Pounce, (a) => new Pounce(a, abilityData[AbilityReference.Pounce]) },
            { AbilityReference.Smash, (a) => new Smash(a, abilityData[AbilityReference.Smash]) },
        };

    private static Dictionary<AbilityReference, Func<Actor, Channel>> channels 
        = new Dictionary<AbilityReference, Func<Actor, Channel>>
        {
            { AbilityReference.Whirlwind, (a) => new Whirlwind(a, abilityData[AbilityReference.Whirlwind]) },
        };

    private static Dictionary<AbilityReference, AbilityData> abilityData
        = new Dictionary<AbilityReference, AbilityData>
        {
            { AbilityReference.Slash, Slash.BaseAbilityData},
            { AbilityReference.Fireball, Fireball.BaseAbilityData},
            { AbilityReference.DaggerThrow, DaggerThrow.BaseAbilityData},
            { AbilityReference.Bite, Bite.BaseAbilityData},
            { AbilityReference.Roll, Roll.BaseAbilityData},
            { AbilityReference.Lunge, Lunge.BaseAbilityData},
            { AbilityReference.Pounce, Pounce.BaseAbilityData},
            { AbilityReference.Smash, Smash.BaseAbilityData},
            { AbilityReference.Whirlwind, Whirlwind.BaseAbilityData},
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

    public static bool TryGetInstantCast(AbilityReference abilityReference, Actor owner, out InstantCast ability)
    {
        if (instantCasts.ContainsKey(abilityReference))
        {
            ability = instantCasts[abilityReference](owner);
            return true;
        }

        ability = null;
        return false;
    }

    public static bool TryGetChannel(AbilityReference abilityReference, Actor owner, out Channel ability)
    {
        if (channels.ContainsKey(abilityReference))
        {
            ability = channels[abilityReference](owner);
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
