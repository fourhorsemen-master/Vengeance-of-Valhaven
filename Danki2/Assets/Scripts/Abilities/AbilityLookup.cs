using System;
using System.Collections.Generic;

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
}
