public abstract class Channel : Ability
{
    public Channel(AbilityContext context) : base(context)
    {
    }
    
    public abstract float Duration { get; }

    public virtual void Start() { }

    public virtual void Continue() { }

    public virtual void Cancel() { }

    public virtual void End() { }
}
