using UnityEngine;

[Ability(AbilityReference.Execute, new[] { "Finishing Touch" })]
public class Execute : Cast
{
    private const float CastRange = 3f;
    private const float PauseDuration = 0.3f;
    private const float AdditionalDamagePerUnit = 10f; // A value of 10f here would increase the additional damage for every 10% health missing.
    private const int AdditionalDamageMultiplier = 3; // The amount of additional damage to do per unit missing health.

    public Execute(Actor owner, AbilityData abilityData, string fmodStartEvent, string fmodEndEvent, string[] availableBonuses, float duration)
        : base(owner, abilityData, fmodStartEvent, fmodEndEvent, availableBonuses, duration)
    {
    }

    public override void End(Actor target)
    {
        Owner.MovementManager.LookAt(target.transform.position);

        float distance = Vector3.Distance(Owner.transform.position, target.transform.position);

        if (distance > CastRange)
        {
            SuccessFeedbackSubject.Next(false);
        }

        int additionalDamageUnits = Mathf.CeilToInt((1f - target.HealthManager.HealthProportion) * 100f / AdditionalDamagePerUnit);
        int additionalDamage = additionalDamageUnits * AdditionalDamageMultiplier;

        DealPrimaryDamage(target, additionalDamage);
        if (HasBonus("Finishing Touch")) target.EffectManager.AddStack(StackingEffect.Bleed);

        Owner.MovementManager.Pause(PauseDuration);
        SuccessFeedbackSubject.Next(true);
    }
}
