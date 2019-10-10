using Assets.Scripts;
using UnityEngine;

namespace Abilities
{
    public abstract class AbilityContext
    {
        private readonly AbilityCaster owner;
        private readonly AbilityCaster target;
        private readonly Vector3 origin;

        public AbilityContext(
            AbilityCaster owner,
            AbilityCaster target,
            Vector3 origin
        )
        {
            this.owner = owner;
            this.target = target;
            this.origin = origin;
        }
    }

    public class Ability
    {
        public Ability(AbilityContext context)
        {
            Context = context;
        }

        public AbilityContext Context { get; }
    }
}
