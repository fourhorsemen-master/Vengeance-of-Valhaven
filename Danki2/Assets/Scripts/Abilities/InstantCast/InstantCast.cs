using System;

public abstract class InstantCast : Ability
{
    protected InstantCast(AbilityContext context) : base(context)
    {
    }

    public abstract void Cast();
}
