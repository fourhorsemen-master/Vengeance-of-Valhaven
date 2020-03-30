using System;
using System.Collections.Generic;

public class Ability
{
    private static Dictionary<AbilityReference, Func<AbilityContext, InstantCast>> _instantCasts = new Dictionary<AbilityReference, Func<AbilityContext, InstantCast>>
    {
        { AbilityReference.Slash, c => new Slash(c) },
        { AbilityReference.Fireball, c => new Fireball(c) },
        { AbilityReference.DaggerThrow, c => new DaggerThrow(c) },
        { AbilityReference.Bite, c => new Bite(c) },
        { AbilityReference.Roll, c => new Roll(c) },
        { AbilityReference.Lunge, c => new Lunge(c) },
        { AbilityReference.Pounce, c => new Pounce(c) },
        { AbilityReference.Smash, c => new Smash(c) },
    };

    private static Dictionary<AbilityReference, Func<AbilityContext, Channel>> _channels = new Dictionary<AbilityReference, Func<AbilityContext, Channel>>
    {
        { AbilityReference.Whirlwind, c => new Whirlwind(c) },
    };

    public AbilityContext Context { get; }

    public Ability(AbilityContext context)
    {
        Context = context;
    }

    public static bool TryGetAsInstantCastBuilder(AbilityReference abilityRef, out Func<AbilityContext, InstantCast> ability)
    {
        bool isInstantCast = _instantCasts.TryGetValue(abilityRef, out ability);
        return isInstantCast;
    }

    public static bool TryGetAsChannelBuilder(AbilityReference abilityRef, out Func<AbilityContext, Channel> ability)
    {
        bool isChannel = _channels.TryGetValue(abilityRef, out ability);
        return isChannel;
    }
}
