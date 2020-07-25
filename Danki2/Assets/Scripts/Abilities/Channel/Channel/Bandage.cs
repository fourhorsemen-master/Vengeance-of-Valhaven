[Ability(AbilityReference.Bandage, new []{"Perseverance"})]
public class Bandage : Channel
{
    public override float Duration => 5f;
    
    public Bandage(Actor owner, AbilityData abilityData, string[] availableBonuses) : base(owner, abilityData, availableBonuses)
    {
    }
}
