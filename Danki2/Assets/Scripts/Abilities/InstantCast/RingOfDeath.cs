using UnityEngine;

[Ability(AbilityReference.RingOfDeath, new [] { "Double Down", "Barbed Daggers" })]
public class RingOfDeath : InstantCast
{
    private const int BaseNumberOfKnives = 12;
    private const int DoubleDownNumberOfKnives = 24;
    private const float KnifeArcAngle = 360f;
    private const float KnifeSpeed = 10f;
    private const float BaseKnifeCastInterval = 0.04f;
    private const float DrawTime = 0.2f;

    private int NumberOfKnives => HasBonus("Double Down") ? DoubleDownNumberOfKnives : BaseNumberOfKnives;
    private float KnifeCastInterval => HasBonus("Double Down") ? BaseKnifeCastInterval / 2 : BaseKnifeCastInterval;

    private int collisionCounter = 0;

    public RingOfDeath(Actor owner, AbilityData abilityData, string fmodStartEvent, string fmodEndEvent, string[] availableBonuses)
        : base(owner, abilityData, fmodStartEvent, fmodEndEvent, availableBonuses)
    {
    }

    public override void Cast(Vector3 floorTargetPosition, Vector3 offsetTargetPosition)
    {
        Owner.MovementManager.LookAt(floorTargetPosition);

        Quaternion rotation = Owner.transform.rotation;

        Owner.MovementManager.Pause(DrawTime + KnifeCastInterval * NumberOfKnives);

        PlayStartEvent();

        for (float i = 0; i < NumberOfKnives; i++)
        {
            float angleOffset = KnifeArcAngle * i / NumberOfKnives;
            Quaternion castRotation = rotation * Quaternion.Euler(Vector3.up * angleOffset);

            Owner.InterruptibleAction(
                DrawTime + KnifeCastInterval * i,
                InterruptionType.Hard,
                () => Throw(castRotation)
            );    
        }
    }

    private void Throw(Quaternion castRotation)
    {
        PlayEndEvent();

        if (HasBonus("Barbed Daggers")) 
        {
            BarbedDaggerObject.Fire(Owner, OnCollision, KnifeSpeed, Owner.AbilitySource, castRotation);
        }
        else
        {
            FanOfKnivesObject.Fire(Owner, OnCollision, KnifeSpeed, Owner.AbilitySource, castRotation);
        }
    }

    private void OnCollision(GameObject gameObject)
    {
        collisionCounter++; // counter to provide failure feedback early if all knives miss.

        if (RoomManager.Instance.TryGetActor(gameObject, out Actor actor))
        {
            if (actor.Opposes(Owner))
            {
                CustomCamera.Instance.AddShake(ShakeIntensity.Low);
                DealPrimaryDamage(actor);
                if (HasBonus("Barbed Daggers")) actor.EffectManager.AddStack(StackingEffect.Bleed);
                SuccessFeedbackSubject.Next(true);
                return;
            }
        }

        if (collisionCounter == NumberOfKnives)
        {
            SuccessFeedbackSubject.Next(false);
        }
    }
}
