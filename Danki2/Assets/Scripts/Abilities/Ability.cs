﻿using System;
using System.Collections.Generic;
using UnityEngine;

public enum AbilityReference
{
    Slash,
    ShieldBash,
    Whirlwind,
}

public class AbilityContext
{
    public AbilityContext(
        Actor owner,
        Actor target,
        Vector3 origin
    )
    {
        Owner = owner;
        Target = target;
        Origin = origin;
    }

    public Actor Owner { get; }
    public Actor Target { get; }
    public Vector3 Origin { get; }
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
    };

    private static Dictionary<AbilityReference, Func<AbilityContext, Channel>> _channels = new Dictionary<AbilityReference, Func<AbilityContext, Channel>>
    {
        { AbilityReference.Whirlwind, c => new Whirlwind(c) },
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
