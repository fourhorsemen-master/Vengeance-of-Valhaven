using System;
using System.Collections.Generic;

public class Ability
{
    private static Dictionary<AbilityReference, Func<AbilityContext, Action<bool>, InstantCast>> instantCasts 
        = new Dictionary<AbilityReference, Func<AbilityContext, Action<bool>, InstantCast>>
    {
        { AbilityReference.Slash, (c, b) => new Slash(c, b) },
        { AbilityReference.Fireball, (c, b) => new Fireball(c, b) },
        { AbilityReference.DaggerThrow, (c, b) => new DaggerThrow(c, b) },
        { AbilityReference.Bite, (c, b) => new Bite(c, b) },
        { AbilityReference.Roll, (c, b) => new Roll(c, b) },
        { AbilityReference.Lunge, (c, b) => new Lunge(c, b) },
        { AbilityReference.Pounce, (c, b) => new Pounce(c, b) },
        { AbilityReference.Smash, (c, b) => new Smash(c, b) },
    };

    private static Dictionary<AbilityReference, Func<AbilityContext, Action<bool>, Channel>> channels 
        = new Dictionary<AbilityReference, Func<AbilityContext, Action<bool>, Channel>>
    {
        { AbilityReference.Whirlwind, (c, b) => new Whirlwind(c, b) },
    };

    protected readonly Action<bool> isSuccessfulCallback;

    public AbilityContext Context { get; }

    public Ability(AbilityContext context, Action<bool> completionCallback)
    {
        Context = context;
        this.isSuccessfulCallback = completionCallback;
    }

    public static bool TryGetInstantCast(
        AbilityReference abilityReference,
        AbilityContext abilityContext,
        Action<bool> completionCallback,
        out InstantCast ability
    )
    {
        if (instantCasts.ContainsKey(abilityReference))
        {
            ability = instantCasts[abilityReference](abilityContext, completionCallback);
            return true;
        }

        ability = null;
        return false;
    }

    public static bool TryGetChannel(
        AbilityReference abilityReference,
        AbilityContext abilityContext,
        Action<bool> completionCallback,
        out Channel ability
    )
    {
        if (channels.ContainsKey(abilityReference))
        {
            ability = channels[abilityReference](abilityContext, completionCallback);
            return true;
        }

        ability = null;
        return false;
    }
}
