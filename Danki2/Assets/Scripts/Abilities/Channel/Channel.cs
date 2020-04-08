using System;

public abstract class Channel : Ability
{
    public abstract float Duration { get; }

    public Channel(AbilityContext context, Action<bool> completionCallback) 
        : base(context, completionCallback)
    {
    }
    
    public virtual void Start() { }

    public virtual void Continue() { }

    public virtual void Cancel() { }

    public virtual void End() { }
}
