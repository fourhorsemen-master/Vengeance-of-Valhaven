using UnityEngine;

[Ability(AbilityReference.Execute, new[] { "Finishing Touch", "Sharpened Edge" })]
public class Execute : Cast
{
    private const float CastRange = 5f;
    private const float PauseDuration = 0.3f;
    private const float HealthPercentageForAdditionalDamage = 0.2f; // A damage boost will occur below this proportion health.
    private const int AdditionalDamageMultiplier = 2; // Multiplier to boost damage by.
    private const int SharpenedEdgeDamageMultiplier = 3;

    private readonly Subject onCastFail = new Subject();
    private readonly Subject<Vector3> onCastComplete = new Subject<Vector3>();

    public Execute(Actor owner, AbilityData abilityData, string fmodStartEvent, string fmodEndEvent, string[] availableBonuses, float duration)
        : base(owner, abilityData, fmodStartEvent, fmodEndEvent, availableBonuses, duration)
    {
    }

    protected override void Start()
    {
        ExecuteObject.Create(Owner.transform.position, Owner.transform.rotation, onCastFail, onCastComplete);
    }

    protected override void Cancel() => onCastFail.Next();

    public override void End(Vector3 floorTargetPosition, Vector3 offsetTargetPosition)
    {
        Owner.MovementManager.LookAt(floorTargetPosition);
        SuccessFeedbackSubject.Next(false);
        onCastFail.Next();
    }

    public override void End(Actor target)
    {
        Owner.MovementManager.LookAt(target.transform.position);

        float distance = Vector3.Distance(Owner.transform.position, target.transform.position);

        if (distance > CastRange)
        {
            SuccessFeedbackSubject.Next(false);
            onCastFail.Next();
            return;
        }

        onCastComplete.Next(target.transform.position);

        int damageMultiplier = 1;

        if (target.HealthManager.HealthProportion <= HealthPercentageForAdditionalDamage)
        {
            damageMultiplier = AdditionalDamageMultiplier;

            if (HasBonus("Sharpened Edge"))
            {
                damageMultiplier = SharpenedEdgeDamageMultiplier;
            }
        }

        DealPrimaryDamage(target, 0, damageMultiplier);
        if (HasBonus("Finishing Touch")) target.EffectManager.AddStack(StackingEffect.Bleed);

        CustomCamera.Instance.AddShake(ShakeIntensity.High);

        Owner.MovementManager.Pause(PauseDuration);
        SuccessFeedbackSubject.Next(true);
    }
}
