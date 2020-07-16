﻿using UnityEngine;

[Ability(AbilityReference.Lunge)]
public class Lunge : InstantCast
{
    private const float MinMovementDuration = 0.1f;
    private const float MaxMovementDuration = 0.2f;
    private const float LungeSpeedMultiplier = 6f;
    private const float StunRange = 2f;
    private const float StunDuration = 0.5f;
    private const float PauseDuration = 0.3f;

    public Lunge(Actor owner, AbilityData abilityData, string[] availableBonuses) : base(owner, abilityData, availableBonuses)
    {
    }

    public override void Cast(Vector3 target)
    {
        Vector3 position = Owner.transform.position;
        Vector3 castDirection = target - Owner.Centre;

        float distance = Vector3.Distance(target, position);
        float lungeSpeed = Owner.GetStat(Stat.Speed) * LungeSpeedMultiplier;
        float duration = Mathf.Clamp(distance/lungeSpeed, MinMovementDuration, MaxMovementDuration);

        Owner.MovementManager.TryLockMovement(MovementLockType.Dash, duration, lungeSpeed, castDirection, castDirection);

        LungeObject lungeObject = LungeObject.Create(position, Quaternion.LookRotation(target - position));

        Owner.InterruptableAction(
            duration,
            InterruptionType.Hard,
            () => DamageOnLand(castDirection, lungeObject)
        );
    }

    private void DamageOnLand(Vector3 castDirection, LungeObject lungeObject)
    {
        Quaternion castRotation = GetMeleeCastRotation(castDirection);

        bool hasDealtDamage = false;

        CollisionTemplateManager.Instance.GetCollidingActors(CollisionTemplate.Wedge90, StunRange, Owner.transform.position, castRotation)
            .Where(actor => actor.Opposes(Owner))
            .ForEach(actor =>
            {
                actor.EffectManager.AddActiveEffect(new Stun(StunDuration), StunDuration);
                DealPrimaryDamage(actor);
                hasDealtDamage = true;
            });

        SuccessFeedbackSubject.Next(hasDealtDamage);
        Owner.MovementManager.Stun(PauseDuration);

        if (hasDealtDamage)
        {
            CustomCamera.Instance.AddShake(ShakeIntensity.Medium);
            lungeObject.PlayHitSound();
        }
    }
}
