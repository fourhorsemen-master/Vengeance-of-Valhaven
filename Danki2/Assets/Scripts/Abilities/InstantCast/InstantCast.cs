using System;

public abstract class InstantCast : Ability
{
    public InstantCast(AbilityContext context, Action<bool> completionCallback) 
        : base(context, completionCallback)
    {
    }

    public abstract void Cast();
}
