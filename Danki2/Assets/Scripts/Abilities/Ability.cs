using System;
using System.Collections.Generic;

public class Ability
{
    private static Dictionary<AbilityReference, Func<AbilityContext, Action<bool>, InstantCast>> _instantCasts 
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

    private static Dictionary<AbilityReference, Func<AbilityContext, Action<bool>, Channel>> _channels 
        = new Dictionary<AbilityReference, Func<AbilityContext, Action<bool>, Channel>>
    {
        { AbilityReference.Whirlwind, (c, b) => new Whirlwind(c, b) },
    };

    protected readonly Action<bool> completionCallback;

    public AbilityContext Context { get; }

    public Ability(AbilityContext context, Action<bool> completionCallback)
    {
        Context = context;
        this.completionCallback = completionCallback;
    }

    public static bool TryGetAsInstantCastBuilder(
        AbilityReference abilityRef, 
        out Func<AbilityContext, Action<bool>, InstantCast> ability
    )
    {
        bool isInstantCast = _instantCasts.TryGetValue(abilityRef, out ability);
        return isInstantCast;
    }

    public static bool TryGetAsChannelBuilder(
        AbilityReference abilityRef, 
        out Func<AbilityContext, Action<bool>, Channel> ability
    )
    {
        bool isChannel = _channels.TryGetValue(abilityRef, out ability);
        return isChannel;
    }
}
