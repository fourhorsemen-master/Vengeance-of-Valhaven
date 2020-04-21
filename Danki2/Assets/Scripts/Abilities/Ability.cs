using System;
using System.Collections.Generic;

public class Ability
{
    private static Dictionary<AbilityReference, Func<AbilityContext, InstantCast>> instantCasts 
        = new Dictionary<AbilityReference, Func<AbilityContext, InstantCast>>
    {
        { AbilityReference.Slash, (c) => new Slash(c) },
        { AbilityReference.Fireball, (c) => new Fireball(c) },
        { AbilityReference.DaggerThrow, (c) => new DaggerThrow(c) },
        { AbilityReference.Bite, (c) => new Bite(c) },
        { AbilityReference.Roll, (c) => new Roll(c) },
        { AbilityReference.Lunge, (c) => new Lunge(c) },
        { AbilityReference.Pounce, (c) => new Pounce(c) },
        { AbilityReference.Smash, (c) => new Smash(c) },
    };

    private static Dictionary<AbilityReference, Func<AbilityContext, Channel>> channels 
        = new Dictionary<AbilityReference, Func<AbilityContext, Channel>>
    {
        { AbilityReference.Whirlwind, (c) => new Whirlwind(c) },
    };

    public Subject<bool> SuccessFeedbackSubject { get; }

    public AbilityContext Context { get; }

    public Ability(AbilityContext context)
    {
        Context = context;
        SuccessFeedbackSubject = new Subject<bool>();
    }

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
