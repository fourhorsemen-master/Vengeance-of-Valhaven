using UnityEngine;

[Ability(AbilityReference.BearCharge)]
public class BearCharge : Channel
{
    private const float ChargeSpeed = 5f;
    private const float ChargeEffectInterval = 0.5f;
    private const float ChargeRotationRate = 0.6f;
    private const float DamageRadius = 2.5f;
    private const float PauseDuration = 0.5f;
    private const float KnockBackDuration = 0.1f;
    private const float KnockBackSpeed = 8f;

    private readonly Repeater repeater;

    private Vector3 direction;

    private bool dealtAnyDamage = false;

    private BearChargeObject chargeObject;
    
    public override ChannelEffectOnMovement EffectOnMovement => ChannelEffectOnMovement.None;

    public BearCharge(Actor owner, AbilityData abilityData, string fmodStartEvent, string fmodEndEvent, string[] availableBonuses, float duration)
        : base(owner, abilityData, fmodStartEvent, fmodEndEvent, availableBonuses, duration)
    {
        repeater = new Repeater(ChargeEffectInterval, ChargeEffect, ChargeEffectInterval);
    }

    public override void Start(Vector3 floorTargetPosition, Vector3 offsetTargetPosition)
    {
        direction = floorTargetPosition - Owner.transform.position;
        chargeObject = BearChargeObject.Create(Owner.transform);
    }

    public override void Continue(Vector3 floorTargetPosition, Vector3 offsetTargetPosition)
    {
        Vector3 desiredDirection = floorTargetPosition - Owner.transform.position;
        direction = Vector3.RotateTowards(
            direction,
            desiredDirection,
            ChargeRotationRate * Time.deltaTime,
            Mathf.Infinity
        );
        Owner.MovementManager.Move(direction, ChargeSpeed);

        repeater.Update();
    }

    public override void Cancel(Vector3 floorTargetPosition, Vector3 offsetTargetPosition) => End();

    public override void End(Vector3 floorTargetPosition, Vector3 offsetTargetPosition) => End();

    private void End()
    {
        SuccessFeedbackSubject.Next(dealtAnyDamage);
        Owner.MovementManager.Pause(PauseDuration);
        chargeObject.Destroy();
    }

    private void ChargeEffect()
    {
        bool hasDealtDamage = false;

        CollisionTemplateManager.Instance.GetCollidingActors(
            CollisionTemplate.Wedge90,
            DamageRadius,
            Owner.CollisionTemplateSource,
            Quaternion.LookRotation(Owner.transform.forward)
        ).ForEach(actor =>
        {
            if (Owner.Opposes(actor))
            {
                KnockBack(actor);
                DealPrimaryDamage(actor);
                hasDealtDamage = true;
            }
        });

        chargeObject.CreateSwipe(
            Owner.AbilitySource,
            GetMeleeCastRotation(Owner.transform.forward),
            hasDealtDamage
        );

        SuccessFeedbackSubject.Next(hasDealtDamage);

        if (hasDealtDamage)
        {
            CustomCamera.Instance.AddShake(ShakeIntensity.Medium);
        }
    }

    private void KnockBack(Actor actor)
    {
        Vector3 knockBackDirection = actor.transform.position - Owner.transform.position;
        Vector3 knockBackFaceDirection = actor.transform.forward;

        actor.MovementManager.TryLockMovement(
            MovementLockType.Knockback,
            KnockBackDuration,
            KnockBackSpeed,
            knockBackDirection,
            knockBackFaceDirection
        );
    }
}
