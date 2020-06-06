using UnityEngine;

[Ability(AbilityReference.Meditate, new []{"Clarity", "GrowingRage"})]
public class Meditate : Charge
{
    protected override float ChargeTime => 5;
    
    public Meditate(Actor owner, AbilityData abilityData, string[] availableBonuses) : base(owner, abilityData, availableBonuses)
    {
    }

    public override void End(Vector3 target) => End();

    public override void End(Actor actor) => End();

    public override void Cancel(Vector3 target) => End();

    public override void Cancel(Actor actor) => End();

    private void End()
    {
        Debug.Log($"Charged for {TimeCharged} seconds.");
    }
}
