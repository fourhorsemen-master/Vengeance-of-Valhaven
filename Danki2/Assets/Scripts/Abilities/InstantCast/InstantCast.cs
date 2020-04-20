using System;

public abstract class InstantCast : Ability
{
    public InstantCast(AbilityContext context) : base(context)
    {
    }

    public abstract void Cast();
}
