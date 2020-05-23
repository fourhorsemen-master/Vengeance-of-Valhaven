[Ability(AbilityReference.ExampleCharge)]
public class ExampleCharge : Charge
{
    public ExampleCharge(Actor owner, AbilityData abilityData, string[] availableBonuses)
        : base(owner, abilityData, availableBonuses)
    {
    }

    public override float Duration => 3f;
}