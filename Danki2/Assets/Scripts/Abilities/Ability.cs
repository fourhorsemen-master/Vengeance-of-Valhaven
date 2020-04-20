public abstract class Ability
{
    public Subject<bool> SuccessFeedbackSubject { get; }

    public AbilityContext Context { get; }

    protected Ability(AbilityContext context)
    {
        Context = context;
        SuccessFeedbackSubject = new Subject<bool>();
    }
}
