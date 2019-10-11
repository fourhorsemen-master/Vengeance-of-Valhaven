using UnityEngine;

namespace Abilities
{
    public abstract class AbilityContext
    {
        private readonly Actor owner;
        private readonly Actor target;
        private readonly Vector3 origin;

        public AbilityContext(
            Actor owner,
            Actor target,
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
