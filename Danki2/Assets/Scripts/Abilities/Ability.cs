using Assets.Scripts.Abilities.Channel;
using Assets.Scripts.Abilities.InstantCast;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Abilities
{
    public enum AbilityReference
    {
        Slash,
        ShieldBash,
        Whirlwind,
    }

    public abstract class AbilityContext
    {
        private readonly Actor _owner;
        private readonly Actor _target;
        private readonly Vector3 _origin;

        public AbilityContext(
            Actor owner,
            Actor target,
            Vector3 origin
        )
        {
            _owner = owner;
            _target = target;
            _origin = origin;
        }
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
}
