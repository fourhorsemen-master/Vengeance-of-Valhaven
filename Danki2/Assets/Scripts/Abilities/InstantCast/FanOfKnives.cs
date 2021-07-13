﻿using UnityEngine;

[Ability(AbilityReference.FanOfKnives, new [] {"Spray"})]
public class FanOfKnives : InstantCast
{
    private const int BaseNumberOfKnives = 3;
    private const int SprayNumberOfKnives = 5;
    private const float KnifeArcAngle = 45f;
    private const float KnifeSpeed = 10f;
    private const float DrawTime = 0.08f;
    private const float KnifeCastInterval = 0.1f;

    private int NumberOfKnives => HasBonus("Spray") ? SprayNumberOfKnives : BaseNumberOfKnives;

    public FanOfKnives(AbilityConstructionArgs arguments) : base(arguments) { }

    public override void Cast(Vector3 floorTargetPosition, Vector3 offsetTargetPosition)
    {
        Quaternion rotation = Quaternion.LookRotation(offsetTargetPosition - Owner.Centre);

        Owner.MovementManager.Pause(DrawTime + KnifeCastInterval * NumberOfKnives);

        Owner.MovementManager.LookAt(offsetTargetPosition);

        PlayVocalisationEvent();
        PlayStartEvent();

        for (float i = 0; i < NumberOfKnives; i++)
        {
            float angleOffset = ((i / (NumberOfKnives - 1)) - 0.5f) * KnifeArcAngle;
            Quaternion castRotation = rotation * Quaternion.Euler(Vector3.up * angleOffset);

            Owner.InterruptibleAction(
                KnifeCastInterval * i + DrawTime,
                InterruptionType.Hard,
                () => Fire(castRotation)
            );  
        }
    }

    private void Fire(Quaternion castRotation)
    {
        PlayEndEvent();
        FanOfKnivesObject.Fire(Owner, OnCollision, KnifeSpeed, Owner.AbilitySource, castRotation);
    }

    private void OnCollision(GameObject gameObject)
    {
        if (ActorCache.Instance.TryGetActor(gameObject, out Actor actor))
        {
            if (actor.Opposes(Owner))
            {
                CustomCamera.Instance.AddShake(ShakeIntensity.Low);
                DealPrimaryDamage(actor);
                return;
            }
        }
    }
}
