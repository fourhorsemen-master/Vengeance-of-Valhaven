using UnityEngine;

[Ability(AbilityReference.Rend)]
public class Rend : Charge
{
    private const int DamageMultiplier = 3;
    private const float DotDuration = 3f;
    
    protected override float ChargeTime => 3f;

    public Rend(Actor owner, AbilityData abilityData, string[] availableBonuses) : base(owner, abilityData, availableBonuses)
    {
    }

    public override void Cancel(Vector3 target) => End();

    public override void End(Vector3 target) => End();

    private void End()
    {
        Debug.Log(TimeCharged);
    }
}
