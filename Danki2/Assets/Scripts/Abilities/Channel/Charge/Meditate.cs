using UnityEngine;

[Ability(AbilityReference.Meditate, new []{"Clarity", "GrowingRage"})]
public class Meditate : Charge
{
    protected override float ChargeTime => 5;

    private const float PowerDuration = 10;
    
    public Meditate(Actor owner, AbilityData abilityData, string[] availableBonuses) : base(owner, abilityData, availableBonuses)
    {
    }

    public override void End(Vector3 target) => End();

    public override void End(Actor actor) => End();

    public override void Cancel(Vector3 target) => End();

    public override void Cancel(Actor actor) => End();

    private void End()
    {
        int powerIncrease = Mathf.FloorToInt(TimeCharged);

        if (powerIncrease == 0)
        {
            SuccessFeedbackSubject.Next(false);
            return;
        }
        
        SuccessFeedbackSubject.Next(true);
        
        StatModification powerModification = new StatModification(Stat.Power, powerIncrease);
        Owner.EffectManager.AddActiveEffect(powerModification, PowerDuration);
    }
}
