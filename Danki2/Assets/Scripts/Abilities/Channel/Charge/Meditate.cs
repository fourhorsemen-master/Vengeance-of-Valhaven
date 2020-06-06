using UnityEngine;

[Ability(AbilityReference.Meditate, new []{"Clarity", "GrowingRage"})]
public class Meditate : Charge
{
    protected override float ChargeTime => 5;

    private const float PowerDuration = 10;
    private const int GrowingRageMultiplier = 2;
    
    public Meditate(Actor owner, AbilityData abilityData, string[] availableBonuses) : base(owner, abilityData, availableBonuses)
    {
    }

    public override void End(Vector3 target) => End();

    public override void End(Actor actor) => End();

    public override void Cancel(Vector3 target) => End();

    public override void Cancel(Actor actor) => End();

    private void End()
    {
        int powerIncrease = GetPowerIncrease();

        if (powerIncrease == 0)
        {
            SuccessFeedbackSubject.Next(false);
            return;
        }
        
        SuccessFeedbackSubject.Next(true);
        StatModification powerModification = new StatModification(Stat.Power, powerIncrease);
        Owner.EffectManager.AddActiveEffect(powerModification, PowerDuration);
    }

    private int GetPowerIncrease()
    {
        return HasBonus("GrowingRage")
            ? Mathf.FloorToInt(TimeCharged) * GrowingRageMultiplier
            : Mathf.FloorToInt(TimeCharged);
    }
}
