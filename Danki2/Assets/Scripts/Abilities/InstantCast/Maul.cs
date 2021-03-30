using UnityEngine;

[Ability(AbilityReference.Maul)]
public class Maul : InstantCast
{
    private const int TotalBiteCount = 3;
    private const float BiteInterval = 0.5f;
    private const float BiteRange = 2.5f;
    private const float SlowDuration = 2f;

    public Maul(AbilityConstructionArgs arguments) : base(arguments) { }

    public override void Cast(Vector3 floorTargetPosition, Vector3 offsetTargetPosition)
    {
        Vector3 castDirection = floorTargetPosition - Owner.transform.position;

        Owner.MovementManager.LookAt(floorTargetPosition);

        MaulObject maulObject = MaulObject.Create(Owner.AbilitySource);

        Owner.InterruptibleIntervalAction(BiteInterval, InterruptionType.Hard, index => Bite(castDirection, index, maulObject), 0f, TotalBiteCount);
    }

    private void Bite(Vector3 castDirection, int index, MaulObject maulObject)
    {
        Vector3 horizontalDirection = Vector3.Cross(castDirection, Vector3.up).normalized;
        int directionMultiplier = index % 2 == 1 ? 1 : -1;
        Vector3 randomisedcastDirection = castDirection.normalized + horizontalDirection * 0.25f * directionMultiplier;

        Quaternion castRotation = GetMeleeCastRotation(randomisedcastDirection);

        maulObject.Bite(castRotation);

        bool hasDealtDamage = false;

        TemplateCollision(
            CollisionTemplate.Wedge45,
            BiteRange,
            Owner.CollisionTemplateSource,
            castRotation,
            actor =>
            {
                DealPrimaryDamage(actor);
                actor.EffectManager.AddActiveEffect(ActiveEffect.Slow, SlowDuration);
                hasDealtDamage = true;
            }
        );

        if (hasDealtDamage)
        {
            CustomCamera.Instance.AddShake(ShakeIntensity.Medium);
            SuccessFeedbackSubject.Next(true);
        }
        else if (index == TotalBiteCount)
        {
            SuccessFeedbackSubject.Next(false);
        }

        Owner.MovementManager.Pause(BiteInterval);
    }
}
