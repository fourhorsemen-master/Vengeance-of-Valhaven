public abstract class Channel : Ability
{
    public Channel(AbilityContext context) : base(context)
    {
    }

    public abstract void Start();

    public abstract void Continue(float xPosition, float yPosition);

    public abstract void Cancel(float xPosition, float yPosition);

    public abstract void End(float xPosition, float yPosition);
}
