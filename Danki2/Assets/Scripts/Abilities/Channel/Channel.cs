public abstract class Channel : Ability
{
    public override AbilityType AbilityType => AbilityType.Channel;

    public abstract float Duration { get; }
    
    public virtual void Start(AbilityContext context) { }

    public virtual void Continue(AbilityContext context) { }

    public virtual void Cancel(AbilityContext context) { }

    public virtual void End(AbilityContext context) { }
}
