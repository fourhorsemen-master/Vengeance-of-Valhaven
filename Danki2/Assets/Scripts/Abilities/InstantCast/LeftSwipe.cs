using UnityEngine;

[Ability(AbilityReference.Swipe)]
public class Swipe : InstantCast
{
    public Swipe(Actor owner, AbilityData abilityData, string[] availableBonuses) : base(owner, abilityData, availableBonuses)
    {
    }

    public override void Cast(Vector3 floorTargetPosition, Vector3 offsetTargetPosition)
    {
        throw new System.NotImplementedException();
    }
}
