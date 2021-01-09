using UnityEngine;

[Ability(AbilityReference.BearCharge)]
public class BearCharge : InstantCast
{
    public BearCharge(Actor owner, AbilityData abilityData, string[] availableBonuses) : base(owner, abilityData, availableBonuses)
    {
    }

    public override void Cast(Vector3 floorTargetPosition, Vector3 offsetTargetPosition)
    {
        Debug.Log("Casting bear charge...");
    }
}
