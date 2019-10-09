using Assets.Scripts;

namespace Abilities
{
    public abstract class AbilityContext
    {
        private readonly AbilityCaster owner;
        private readonly AbilityCaster target;
        private readonly float xOrigin;
        private readonly float yOrigin;

        public AbilityContext(
            AbilityCaster owner,
            AbilityCaster target,
            float xOrigin,
            float yOrigin
        )
        {
            this.owner = owner;
            this.target = target;
            this.xOrigin = xOrigin;
            this.yOrigin = yOrigin;
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
