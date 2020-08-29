using UnityEngine;

[Ability(AbilityReference.FanOfKnives)]
public class FanOfKnives : InstantCast
{
    private const float swordSpeed = 10f;
    private const float poisonSwordDOTLength = 5f;

    public FanOfKnives(Actor owner, AbilityData abilityData, string[] availableBonuses) : base(owner, abilityData, availableBonuses)
    {
    }

    public override void Cast(Vector3 target)
    {
        CustomCamera.Instance.AddShake(ShakeIntensity.Low);

        Quaternion rotation = Quaternion.LookRotation(target - Owner.Centre);
        SwordThrowObject.Fire(Owner, OnCollision, swordSpeed, Owner.Centre, rotation);
    }

    private void OnCollision(GameObject gameObject)
    {
        CustomCamera.Instance.AddShake(ShakeIntensity.Medium);

        if (gameObject.IsActor())
        {
            Actor actor = gameObject.GetComponent<Actor>();

            if (!actor.Opposes(Owner))
            {
                SuccessFeedbackSubject.Next(false);
                return;
            }

            DealPrimaryDamage(actor);
            if (HasBonus("Poison Sword")) ApplySecondaryDamageAsDOT(actor, poisonSwordDOTLength);

            SuccessFeedbackSubject.Next(true);
        }
        else
        {
            SuccessFeedbackSubject.Next(false);
        }
    }
}
