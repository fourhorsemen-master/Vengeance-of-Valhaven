using UnityEngine;

[Ability(AbilityReference.Meditate)]
public class Meditate : Charge
{
    protected override float ChargeTime => 5;
    
    public Meditate(Actor owner, AbilityData abilityData, string[] availableBonuses) : base(owner, abilityData, availableBonuses)
    {
    }

    public override void End(Vector3 target)
    {
        Debug.Log("Ending meditate");
    }

    public override void Cancel(Vector3 target)
    {
        Debug.Log("Cancelling meditate");
    }
}
