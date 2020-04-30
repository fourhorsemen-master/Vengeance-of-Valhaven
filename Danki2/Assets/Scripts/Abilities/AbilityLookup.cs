using System;
using System.Collections.Generic;
using UnityEngine;

public static class AbilityLookup
{
    private static readonly Dictionary<AbilityReference, Func<Actor, AbilityData, InstantCast>> instantCasts 
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

    private static readonly Dictionary<AbilityReference, Func<Actor, AbilityData, Channel>> channels
        = new Dictionary<AbilityReference, Func<Actor, AbilityData, Channel>>
        {
            { AbilityReference.Whirlwind, (a, b) => new Whirlwind(a, b) },
        };

    // Populated by the static constructor based on the instant casts and channels lookups
    private static readonly Dictionary<AbilityReference, AbilityType> abilityTypes = new Dictionary<AbilityReference, AbilityType>();

    private static readonly Dictionary<AbilityReference, AbilityData> abilityDataLookup
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

    private static readonly Dictionary<AbilityReference, Dictionary<OrbType, int>> generatedOrbsLookup
        = new Dictionary<AbilityReference, Dictionary<OrbType, int>>()
        {
            { AbilityReference.Slash, Slash.GeneratedOrbs },
            { AbilityReference.Fireball, Fireball.GeneratedOrbs },
            { AbilityReference.DaggerThrow, DaggerThrow.GeneratedOrbs },
            { AbilityReference.Bite, Bite.GeneratedOrbs },
            { AbilityReference.Roll, Roll.GeneratedOrbs },
            { AbilityReference.Lunge, Lunge.GeneratedOrbs },
            { AbilityReference.Pounce, Pounce.GeneratedOrbs },
            { AbilityReference.Smash, Smash.GeneratedOrbs },
            { AbilityReference.Whirlwind, Whirlwind.GeneratedOrbs },
        };

    private static readonly Dictionary<AbilityReference, OrbType> abilityOrbTypeLookup
        = new Dictionary<AbilityReference, OrbType>()
        {
            { AbilityReference.Slash, Slash.AbilityOrbType },
            { AbilityReference.Fireball, Fireball.AbilityOrbType },
            { AbilityReference.DaggerThrow, DaggerThrow.AbilityOrbType },
            { AbilityReference.Bite, Bite.AbilityOrbType },
            { AbilityReference.Roll, Roll.AbilityOrbType },
            { AbilityReference.Lunge, Lunge.AbilityOrbType },
            { AbilityReference.Pounce, Pounce.AbilityOrbType },
            { AbilityReference.Smash, Smash.AbilityOrbType },
            { AbilityReference.Whirlwind, Whirlwind.AbilityOrbType },
        };

    static AbilityLookup()
    {
        foreach (AbilityReference abilityReference in Enum.GetValues(typeof(AbilityReference)))
        {
            if (!abilityDataLookup.ContainsKey(abilityReference)) Debug.LogError($"No ability data for {abilityReference.ToString()}");
            if (!generatedOrbsLookup.ContainsKey(abilityReference)) Debug.LogError($"No generated orbs for {abilityReference.ToString()}");
            if (!abilityOrbTypeLookup.ContainsKey(abilityReference)) Debug.LogError($"No ability orb type for {abilityReference.ToString()}");

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

    public static Dictionary<OrbType, int> GetGeneratedOrbs(AbilityReference abilityReference)
    {
        return generatedOrbsLookup[abilityReference];
    }

    public static OrbType GetAbilityOrbType(AbilityReference abilityReference)
    {
        return abilityOrbTypeLookup[abilityReference];
    }
}
