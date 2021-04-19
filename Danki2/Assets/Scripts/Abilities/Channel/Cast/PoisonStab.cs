﻿using UnityEngine;

[Ability(AbilityReference.PoisonStab, new []{ "Potent" })]
public class PoisonStab : Cast
{
    private const float MovementSpeedMultiplier = 6f;
    private const float MovementDuration = 0.1f;
    private const float PauseDuration = 0.3f;
    private const float Range = 2f;
    private const float PoisonDuration = 10f;
    private const float PotentPoisonDuration = 20f;

    private readonly Subject onCastFail = new Subject();
    private readonly Subject onFinishMovement = new Subject();

    private bool HasPotent => HasBonus("Potent");

    public PoisonStab(AbilityConstructionArgs arguments) : base(arguments) {}

    protected override void Start()
    {
        PoisonStabObject.Create(Owner.AbilitySourceTransform, onCastFail, onFinishMovement);
    }

    protected override void Cancel()
    {
        onCastFail.Next();
    }

    public override void End(Vector3 floorTargetPosition, Vector3 offsetTargetPosition)
    {
        Vector3 position = Owner.transform.position;
        Vector3 castDirection = floorTargetPosition - position;

        float lungeSpeed = Owner.StatsManager.Get(Stat.Speed) * MovementSpeedMultiplier;

        Owner.MovementManager.TryLockMovement(
            MovementLockType.Dash,
            MovementDuration,
            lungeSpeed,
            castDirection,
            castDirection
        );

        Owner.StartTrail(MovementDuration + PauseDuration);

        Owner.InterruptibleAction(
            MovementDuration,
            InterruptionType.Hard,
            () => PoisonOnLand(castDirection)
        );
    }

    private void PoisonOnLand(Vector3 castDirection)
    {
        onFinishMovement.Next();

        Quaternion castRotation = GetMeleeCastRotation(castDirection);

        bool hasAppliedPoison = false;

        TemplateCollision(
            CollisionTemplateShape.Wedge45,
            Range,
            Owner.CollisionTemplateSource,
            castRotation,
            actor =>
            {
                actor.EffectManager.AddActiveEffect(
                    ActiveEffect.Poison,
                    HasPotent ? PotentPoisonDuration : PoisonDuration
                );
                DealPrimaryDamage(actor);
                hasAppliedPoison = true;
            },
            CollisionSoundLevel.Low
        );

        SuccessFeedbackSubject.Next(hasAppliedPoison);
        Owner.MovementManager.Pause(PauseDuration);

        if (hasAppliedPoison)
        {
            CustomCamera.Instance.AddShake(ShakeIntensity.Medium);
        }
    }
}