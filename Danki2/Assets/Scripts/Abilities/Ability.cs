public abstract class Ability
{
    public Subject<bool> SuccessFeedbackSubject { get; }

    public Actor Owner { get; }
    
    public AbilityData AbilityData { get; }

    protected Ability(Actor owner, AbilityData abilityData)
    {
        Owner = owner;
        AbilityData = abilityData;
        SuccessFeedbackSubject = new Subject<bool>();
    }
}
