using System.Linq;
using UnityEngine;

public abstract class Ability
{
    // Max angle and min vertical angles you can target with melee attacks
    public const float MaxMeleeVerticalAngle = 30f;
    public const float MinMeleeVerticalAngle = -30f;

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

    protected void DealPrimaryDamage(Actor target, int damageModifier = 0)
    {
        Owner.DamageTarget(target, AbilityData.PrimaryDamage + damageModifier);
    }

    protected void ApplyPrimaryDamageAsDOT(
        Actor target,
        float duration,
        float tickRate = 1,
        int linearDamageModifier = 0,
        int multiplicativeDamageModifier = 1
    )
    {
        int totalDamage = (AbilityData.PrimaryDamage + linearDamageModifier) * multiplicativeDamageModifier;
        target.EffectManager.AddActiveEffect(new DOT(totalDamage, duration, tickRate), duration);
    }

    protected void DealSecondaryDamage(Actor target, int damageModifier = 0)
    {
        Owner.DamageTarget(target, AbilityData.SecondaryDamage + damageModifier);
    }

    protected void ApplySecondaryDamageAsDOT(Actor target, float duration, float tickRate = 1, int damageModifier = 0)
    {
        target.EffectManager.AddActiveEffect(new DOT(AbilityData.SecondaryDamage + damageModifier, duration, tickRate), duration);
    }

    protected void Heal(int healModifier = 0)
    {
        Owner.HealthManager.ReceiveHeal(AbilityData.Heal + healModifier);
    }

    protected bool HasBonus(string bonus)
    {
        return ActiveBonuses.Contains(bonus);
    }

    protected Quaternion GetMeleeCastRotation(Vector3 castDirection)
    {
        var castRotation = Quaternion.LookRotation(castDirection);
        var castAngleX = castRotation.eulerAngles.x;

        if (castAngleX > 180f) castAngleX -= 360f;

        float newAngleX = Mathf.Clamp(castAngleX, MinMeleeVerticalAngle, MaxMeleeVerticalAngle);

        return Quaternion.Euler(newAngleX, castRotation.eulerAngles.y, castRotation.eulerAngles.z);
    }
}
