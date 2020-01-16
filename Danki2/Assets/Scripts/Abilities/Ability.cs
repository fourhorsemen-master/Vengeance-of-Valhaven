using System;
using System.Collections.Generic;

public enum AbilityReference
{
    Slash,
    ShieldBash,
    Whirlwind,
    Fireball,
    Bite,
    Pounce,
}

public class Ability
{
    public Ability(AbilityContext context)
    {
        Context = context;
    }

    public AbilityContext Context { get; }

    private static Dictionary<AbilityReference, Func<AbilityContext, InstantCast>> _instantCasts = new Dictionary<AbilityReference, Func<AbilityContext, InstantCast>>
    {
        { AbilityReference.Slash, c => new Slash(c) },
        { AbilityReference.ShieldBash, c => new ShieldBash(c) },
        { AbilityReference.Fireball, c => new Fireball(c) },
        { AbilityReference.Bite, c => new Bite(c) },
    };

    private static Dictionary<AbilityReference, Func<AbilityContext, Channel>> _channels = new Dictionary<AbilityReference, Func<AbilityContext, Channel>>
    {
        { AbilityReference.Whirlwind, c => new Whirlwind(c) },
        { AbilityReference.Pounce, c => new Pounce(c) },
    };

    public static bool TryGetAsInstantCastBuilder(AbilityReference abilityRef, out Func<AbilityContext, InstantCast> ability)
    {
        var isInstantCast = _instantCasts.TryGetValue(abilityRef, out ability);
        return isInstantCast;
    }

    public static bool TryGetAsChannelBuilder(AbilityReference abilityRef, out Func<AbilityContext, Channel> ability)
    {
        var isChannel = _channels.TryGetValue(abilityRef, out ability);
        return isChannel;
    }
}
