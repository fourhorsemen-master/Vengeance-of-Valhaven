using UnityEngine;

[Ability(AbilityReference.RingOfDeath, new [] { "Double Down", "Barbed Daggers" })]
public class RingOfDeath : InstantCast
{
    private const int baseNumberOfKnives = 8;
    private const int doubleDownNumberOfKnives = 16;
    private const float baseKnifeArcAngle = 315f;
    private const float doubleDownKnifeArcAngle = 675f;
    private const float knifeSpeed = 10f;
    private const float knifeCastInterval = 0.07f;

    private int NumberOfKnives => HasBonus("Double Down") ? doubleDownNumberOfKnives : baseNumberOfKnives;
    private float KnifeArcAngle => HasBonus("Double Down") ? doubleDownKnifeArcAngle : baseKnifeArcAngle;

    private int collisionCounter = 0;

    public RingOfDeath(Actor owner, AbilityData abilityData, string fmodStartEvent, string fmodEndEvent, string[] availableBonuses)
        : base(owner, abilityData, fmodStartEvent, fmodEndEvent, availableBonuses)
    {
    }

    public override void Cast(Vector3 floorTargetPosition, Vector3 offsetTargetPosition)
    {
        Owner.MovementManager.LookAt(floorTargetPosition);

        Quaternion rotation = Quaternion.LookRotation(offsetTargetPosition - Owner.Centre);

        Owner.MovementManager.Pause(knifeCastInterval * NumberOfKnives);

        for (float i = 0; i < NumberOfKnives; i++)
        {
            float angleOffset = (i / (NumberOfKnives - 1)) * KnifeArcAngle;
            Quaternion castRotation = rotation * Quaternion.Euler(Vector3.up * angleOffset);

            Owner.WaitAndAct(
                knifeCastInterval * i,
                () => Throw(castRotation)
            );    
        }
    }

    private void Throw(Quaternion castRotation)
    {
        if(HasBonus("Barbed Daggers"))
        {
            FanOfKnivesObject.Fire(Owner, OnCollision, knifeSpeed, Owner.AbilitySource, castRotation);
        }
        else
        {
            BarbedDaggerObject.Fire(Owner, OnCollision, knifeSpeed, Owner.AbilitySource, castRotation);
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
