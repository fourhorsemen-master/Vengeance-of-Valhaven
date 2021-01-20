using UnityEngine;

[Ability(AbilityReference.Maul)]
public class Maul : InstantCast
{
    public Maul(Actor owner, AbilityData abilityData, string[] availableBonuses) : base(owner, abilityData, availableBonuses)
    {
    }

    public override void Cast(Vector3 floorTargetPosition, Vector3 offsetTargetPosition)
    {
        throw new System.NotImplementedException();
    }
}
