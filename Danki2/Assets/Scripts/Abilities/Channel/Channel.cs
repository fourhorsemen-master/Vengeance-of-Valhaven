public abstract class Channel : Ability
{
    public Channel(AbilityContext context) : base(context)
    {
    }
    
    public abstract float Duration { get; }

    public abstract void Start();

    public abstract void Continue();

    public abstract void Cancel();

    public abstract void End();
}
