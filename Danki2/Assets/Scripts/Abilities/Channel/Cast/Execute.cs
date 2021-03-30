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
    private readonly Subject<Actor> onCastComplete = new Subject<Actor>();

    public Execute(AbilityContructionArgs arguments) : base(arguments) { }

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

        onCastComplete.Next(target);

        int damageMultiplier = target.HealthManager.HealthProportion <= HealthPercentageForAdditionalDamage
            ? HasBonus("Sharpened Edge") ? SharpenedEdgeDamageMultiplier : AdditionalDamageMultiplier
            : 1;

        DealPrimaryDamage(target, 0, damageMultiplier);
        if (HasBonus("Finishing Touch")) target.EffectManager.AddStack(StackingEffect.Bleed);

        CustomCamera.Instance.AddShake(ShakeIntensity.High);

        Owner.MovementManager.Pause(PauseDuration);
        SuccessFeedbackSubject.Next(true);
    }
}
