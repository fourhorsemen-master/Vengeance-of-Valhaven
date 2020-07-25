using UnityEngine;

[Ability(AbilityReference.IntimidatingShout)]
public class IntimidatingShout : InstantCast
{
    public IntimidatingShout(Actor owner, AbilityData abilityData, string[] availableBonuses) : base(owner, abilityData, availableBonuses)
    {
    }

    public override void Cast(Vector3 target)
    {
        Debug.Log("Intimidating shout...");
    }
}
