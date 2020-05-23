[Ability(AbilityReference.ExampleCast)]
public class ExampleCast : Cast
{
    public ExampleCast(Actor owner, AbilityData abilityData, string[] availableBonuses)
        : base(owner, abilityData, availableBonuses)
    {
    }

    public override float Duration => 3f;
}