using System;
using System.Collections.Generic;

public static class AbilityLookup
{
    private static Dictionary<AbilityReference, Func<AbilityContext, InstantCast>> instantCasts 
        = new Dictionary<AbilityReference, Func<AbilityContext, InstantCast>>
        {
            { AbilityReference.Slash, (c) => new Slash(c, abilityData[AbilityReference.Slash]) },
            { AbilityReference.Fireball, (c) => new Fireball(c, abilityData[AbilityReference.Fireball]) },
            { AbilityReference.DaggerThrow, (c) => new DaggerThrow(c, abilityData[AbilityReference.DaggerThrow]) },
            { AbilityReference.Bite, (c) => new Bite(c, abilityData[AbilityReference.Bite]) },
            { AbilityReference.Roll, (c) => new Roll(c, abilityData[AbilityReference.Roll]) },
            { AbilityReference.Lunge, (c) => new Lunge(c, abilityData[AbilityReference.Lunge]) },
            { AbilityReference.Pounce, (c) => new Pounce(c, abilityData[AbilityReference.Pounce]) },
            { AbilityReference.Smash, (c) => new Smash(c, abilityData[AbilityReference.Smash]) },
        };

    private static Dictionary<AbilityReference, Func<AbilityContext, Channel>> channels 
        = new Dictionary<AbilityReference, Func<AbilityContext, Channel>>
        {
            { AbilityReference.Whirlwind, (c) => new Whirlwind(c, abilityData[AbilityReference.Whirlwind]) },
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

    public static bool TryGetInstantCast(AbilityReference abilityReference, AbilityContext abilityContext, out InstantCast ability)
    {
        if (instantCasts.ContainsKey(abilityReference))
        {
            ability = instantCasts[abilityReference](abilityContext);
            return true;
        }

        ability = null;
        return false;
    }

    public static bool TryGetChannel(AbilityReference abilityReference, AbilityContext abilityContext, out Channel ability)
    {
        if (channels.ContainsKey(abilityReference))
        {
            ability = channels[abilityReference](abilityContext);
            return true;
        }

        ability = null;
        return false;
    }
}
