using System;

public abstract class Channel : Ability
{
    public abstract float Duration { get; }

    protected Channel(AbilityContext context) : base(context)
    {
    }
    
    public virtual void Start() { }

    public virtual void Continue() { }

    public virtual void Cancel() { }

    public virtual void End() { }
}
