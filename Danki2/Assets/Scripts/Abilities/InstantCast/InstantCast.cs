using Abilities;

namespace Assets.Scripts.Abilities.InstantCast
{
    public abstract class InstantCast : Ability
    {
        public InstantCast(AbilityContext context) : base(context)
        {
        }

        public abstract void Cast();
    }
}
