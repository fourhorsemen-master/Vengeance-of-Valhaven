using Abilities;
using Assets.Scripts.Abilities.InstantCast;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.Abilities
{
    public enum AbilityReference
    {
        Slash,
        ShieldBash,
    }

    public class AbilityBuilder
    {
        private readonly Func<AbilityContext, Ability> _buildAction;

        public AbilityBuilder(Func<AbilityContext, Ability> buildAction)
        {
            _buildAction = buildAction;
        }

        public Ability Build(AbilityContext context)
        {
            return _buildAction(context);
        }

        // This is where we tie the AbilityReference enum to abilities.
        private static Dictionary<AbilityReference, AbilityBuilder> abilityBuilders = new Dictionary<AbilityReference, AbilityBuilder>
        {
            { AbilityReference.Slash, new AbilityBuilder(c => new Slash(c)) },
            { AbilityReference.ShieldBash, new AbilityBuilder(c => new ShieldBash(c)) }
        };

        public static AbilityBuilder GetAbilityBuilder(AbilityReference reference)
        {
            if (!abilityBuilders.TryGetValue(reference, out var builder))
            {
                throw new KeyNotFoundException($"No builder registered for ability {reference.ToString()}");
            }

            return builder;
        }
    }
}
