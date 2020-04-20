using System;

public abstract class InstantCast : Ability
{
    protected InstantCast(AbilityContext context, AbilityData abilityData) : base(context, abilityData)
    {
    }

    public abstract void Cast();
}
