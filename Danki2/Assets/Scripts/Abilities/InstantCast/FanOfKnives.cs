using UnityEngine;

[Ability(AbilityReference.FanOfKnives)]
public class FanOfKnives : InstantCast
{
    private const float knifeArcAngle = 30f;
    private const float knifeSpeed = 10f;

    private int collisionCounter = 0;

    public FanOfKnives(Actor owner, AbilityData abilityData, string[] availableBonuses) : base(owner, abilityData, availableBonuses)
    {
    }

    public override void Cast(Vector3 target)
    {
        Quaternion rotation1 = Quaternion.LookRotation(target - Owner.Centre);

        Quaternion rotation2 = rotation1;
        rotation2 *= Quaternion.Euler(Vector3.up * knifeArcAngle);

        Quaternion rotation3 = rotation1;
        rotation3 *= Quaternion.Euler(Vector3.up * -knifeArcAngle);

        FanOfKnivesObject.Fire(Owner, OnCollision, knifeSpeed, Owner.Centre, rotation1, true);
        FanOfKnivesObject.Fire(Owner, OnCollision, knifeSpeed, Owner.Centre, rotation2);
        FanOfKnivesObject.Fire(Owner, OnCollision, knifeSpeed, Owner.Centre, rotation3);
    }

    private void OnCollision(GameObject gameObject)
    {
        collisionCounter++;        

        if (gameObject.IsActor())
        {
            Actor actor = gameObject.GetComponent<Actor>();

            if (!actor.Opposes(Owner))
            {
                if (collisionCounter == 3)
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
            if (collisionCounter == 3)
            {
                SuccessFeedbackSubject.Next(false);
            }
        }
    }
}
