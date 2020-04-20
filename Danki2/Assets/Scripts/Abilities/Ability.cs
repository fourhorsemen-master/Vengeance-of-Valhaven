public class Ability
{
    public Subject<bool> SuccessFeedbackSubject { get; }

    public AbilityContext Context { get; }

    public Ability(AbilityContext context)
    {
        Context = context;
        SuccessFeedbackSubject = new Subject<bool>();
    }
}
