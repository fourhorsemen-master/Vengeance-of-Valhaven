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

    /// The return boolean value indicates whether the cast was successful.
    public abstract bool Cancel();

    /// The return boolean value indicates whether the cast was successful.
    public abstract bool End();
}
