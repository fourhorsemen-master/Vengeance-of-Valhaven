using UnityEngine;

[Ability(AbilityReference.FanOfKnives, new [] {"Spray"})]
public class FanOfKnives : InstantCast
{
    private const int baseNumberOfKnives = 3;
    private const int sprayNumberOfKnives = 5;
    private const float knifeArcAngle = 45f;
    private const float knifeSpeed = 10f;
    private const float knifeCastInterval = 0.07f;

    private int NumberOfKnives => HasBonus("Spray") ? sprayNumberOfKnives : baseNumberOfKnives;

    private int collisionCounter = 0;

    public FanOfKnives(Actor owner, AbilityData abilityData, string[] availableBonuses) : base(owner, abilityData, availableBonuses)
    {
    }

    public override void Cast(Vector3 _, Vector3 offsetTargetPosition)
    {
        Quaternion rotation = Quaternion.LookRotation(offsetTargetPosition - Owner.Centre);

        Owner.MovementManager.Pause(knifeCastInterval * NumberOfKnives);

        for (float i = 0; i < NumberOfKnives; i++)
        {
            float angleOffset = ((i / (NumberOfKnives - 1)) - 0.5f) * knifeArcAngle;
            Quaternion castRotation = rotation * Quaternion.Euler(Vector3.up * angleOffset);

            Owner.WaitAndAct(
                knifeCastInterval * i,
                () => FanOfKnivesObject.Fire(Owner, OnCollision, knifeSpeed, Owner.Centre, castRotation)
                );    
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
