﻿using UnityEngine;

[Ability(AbilityReference.BearCharge)]
public class BearCharge : Channel
{
    private const float ChargeSpeed = 4f;
    private const float ChargeEffectInterval = 0.5f;
    private const float ChargeRotationRate = 0.8f;
    private const float DamageRadius = 2.5f;
    private const float PauseDuration = 0.5f;

    private readonly Repeater repeater;

    private Vector3 direction;

    private bool dealtAnyDamage = false;
    
    public override ChannelEffectOnMovement EffectOnMovement => ChannelEffectOnMovement.None;

    public BearCharge(Actor owner, AbilityData abilityData, string[] availableBonuses, float duration) : base(owner, abilityData, availableBonuses, duration)
    {
        repeater = new Repeater(ChargeEffectInterval, ChargeEffect, ChargeEffectInterval);
    }

    public override void Start(Vector3 floorTargetPosition, Vector3 offsetTargetPosition)
    {
        direction = floorTargetPosition - Owner.transform.position;
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
    }

    private void ChargeEffect()
    {
        bool hasDealtDamage = false;

        CollisionTemplateManager.Instance.GetCollidingActors(
            CollisionTemplate.Wedge90,
            DamageRadius,
            Owner.transform.position,
            Quaternion.LookRotation(Owner.transform.forward)
        ).ForEach(actor =>
        {
            if (Owner.Opposes(actor))
            {
                DealPrimaryDamage(actor);
                hasDealtDamage = true;
            }
        });

        // TODO: use actor.AbilitySource when that exists - rather than translating from centre
        var swipeObject = SwipeObject.Create(Owner.Centre + 3 * Owner.transform.forward, GetMeleeCastRotation(Owner.transform.forward));

        SuccessFeedbackSubject.Next(hasDealtDamage);

        if (hasDealtDamage)
        {
            swipeObject.PlayHitSound();
            CustomCamera.Instance.AddShake(ShakeIntensity.Medium);
        }
    }
}
