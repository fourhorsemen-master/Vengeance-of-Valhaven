using UnityEngine;

[Ability(AbilityReference.FanOfKnives)]
public class FanOfKnives : InstantCast
{
    private const int numberOfKnives = 3;
    private const float knifeArcAngle = 45f;
    private const float knifeSpeed = 10f;
    private const float knifeCastInterval = 0.07f;

    private int collisionCounter = 0;

    public FanOfKnives(Actor owner, AbilityData abilityData, string[] availableBonuses) : base(owner, abilityData, availableBonuses)
    {
    }

    public override void Cast(Vector3 target)
    {
        Quaternion rotation = Quaternion.LookRotation(target - Owner.Centre);

        Owner.MovementManager.Pause(knifeCastInterval * numberOfKnives);

        for (float i = 0; i < numberOfKnives; i++)
        {
            float angleOffset = ((i / (numberOfKnives - 1)) - 0.5f) * knifeArcAngle;
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

        if (gameObject.IsActor())
        {
            Actor actor = gameObject.GetComponent<Actor>();

            if (!actor.Opposes(Owner))
            {
                if (collisionCounter == numberOfKnives)
                {
                    SuccessFeedbackSubject.Next(false);
                }                
                return;
            }

            CustomCamera.Instance.AddShake(ShakeIntensity.Low);
            DealPrimaryDamage(actor);
            SuccessFeedbackSubject.Next(true);
        }
        else
        {
            if (collisionCounter == numberOfKnives)
            {
                SuccessFeedbackSubject.Next(false);
            }
        }
    }
}
