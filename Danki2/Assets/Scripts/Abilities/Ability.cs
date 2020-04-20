public abstract class Ability
{
    public Subject<bool> SuccessFeedbackSubject { get; }

    public AbilityContext Context { get; }
    
    public AbilityData AbilityData { get; }

    protected Ability(AbilityContext context, AbilityData abilityData)
    {
        Context = context;
        AbilityData = abilityData;
        SuccessFeedbackSubject = new Subject<bool>();
    }
}
