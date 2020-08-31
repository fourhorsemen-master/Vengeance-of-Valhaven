using UnityEngine;

[Ability(AbilityReference.Hamstring, new []{"Hack"})]
public class Hamstring : InstantCast
{
    public Hamstring(Actor owner, AbilityData abilityData, string[] availableBonuses) : base(owner, abilityData, availableBonuses)
    {
    }

    public override void Cast(Vector3 target)
    {
        Debug.Log("Casting hamstring");
        HamstringObject.Create(Owner.transform);
    }
}
