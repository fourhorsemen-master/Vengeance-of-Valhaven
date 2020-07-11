using UnityEngine;

[Ability(AbilityReference.Parry)]
public class Parry : InstantCast
{
    public Parry(Actor owner, AbilityData abilityData, string[] availableBonuses) : base(owner, abilityData, availableBonuses)
    {
    }

    public override void Cast(Vector3 target)
    {
        Debug.Log("Casting Parry...");
    }
}
