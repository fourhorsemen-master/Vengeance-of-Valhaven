using UnityEngine;

namespace Abilities
{
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
    }
}
