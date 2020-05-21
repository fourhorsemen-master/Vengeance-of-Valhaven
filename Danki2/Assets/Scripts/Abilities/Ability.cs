using System.Linq;

public abstract class Ability
{
    public Subject<bool> SuccessFeedbackSubject { get; }

    protected Actor Owner { get; }
    
    private AbilityData AbilityData { get; }

    private string[] ActiveBonuses { get; }

    protected Ability(Actor owner, AbilityData abilityData, string[] activeBonuses)
    {
        Owner = owner;
        AbilityData = abilityData;
        ActiveBonuses = activeBonuses;
        SuccessFeedbackSubject = new Subject<bool>();
    }

    protected void DealPrimaryDamage(Actor target)
    {
        Owner.DamageTarget(target, AbilityData.PrimaryDamage);
    }

    protected void ApplyPrimaryDamageAsDOT(Actor target, float duration, float tickRate = 1)
    {
        target.EffectManager.AddActiveEffect(new DOT(AbilityData.PrimaryDamage, duration, tickRate), duration);
    }

    protected void DealSecondaryDamage(Actor target)
    {
        Owner.DamageTarget(target, AbilityData.SecondaryDamage);
    }

    protected void ApplySecondaryDamageAsDOT(Actor target, float duration, float tickRate = 1)
    {
        target.EffectManager.AddActiveEffect(new DOT(AbilityData.SecondaryDamage, duration, tickRate), duration);
    }

    protected void Heal()
    {
        Owner.HealthManager.ReceiveHeal(AbilityData.Heal);
    }

    protected bool HasBonus(string bonus)
    {
        return ActiveBonuses.Contains(bonus);
    }
}
