using UnityEngine;

[Ability(AbilityReference.Sprint)]
public class Sprint : Cast
{
    public override float Duration => 1;

    private const float SpeedModification = 2;
    private const float SprintDuration = 6;

    public Sprint(Actor owner, AbilityData abilityData, string[] availableBonuses)
        : base(owner, abilityData, availableBonuses)
    {
    }

    public override void End(Vector3 target)
    {
        Owner.EffectManager.AddActiveEffect(new SpeedModifier(2), SprintDuration);
    }
}
