public abstract class Ability
{
    public Subject<bool> SuccessFeedbackSubject { get; }

    protected Actor Owner { get; }
    
    private AbilityData AbilityData { get; }

    protected Ability(Actor owner, AbilityData abilityData)
    {
        Owner = owner;
        AbilityData = abilityData;
        SuccessFeedbackSubject = new Subject<bool>();
    }

    protected void DealPrimaryDamage(Actor target)
    {
        Owner.DamageTarget(target, AbilityData.PrimaryDamage);
    }

    protected void DealPrimaryDOT(Actor target, float tickRate, float duration)
    {
        target.EffectManager.AddActiveEffect(new DOT(AbilityData.PrimaryDamage, tickRate), duration);
    }

    protected void DealSecondaryDamage(Actor target)
    {
        Owner.DamageTarget(target, AbilityData.SecondaryDamage);
    }

    protected void DealSecondaryDOT(Actor target, float tickRate, float duration)
    {
        target.EffectManager.AddActiveEffect(new DOT(AbilityData.SecondaryDamage, tickRate), duration);
    }

    protected void Heal()
    {
        Owner.HealthManager.ReceiveHeal(AbilityData.Heal);
    }
}
