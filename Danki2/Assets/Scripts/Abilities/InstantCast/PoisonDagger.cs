using UnityEngine;

[Ability(AbilityReference.PoisonDagger)]
public class PoisonDagger : InstantCast
{
    private const float DaggerSpeed = 20f;
    private const float DotDuration = 5f;

    public PoisonDagger(Actor owner, AbilityData abilityData, string[] availableBonuses) : base(owner, abilityData, availableBonuses)
    {
    }

    public override void Cast(Vector3 target)
    {
        Quaternion rotation = Quaternion.LookRotation(target - Owner.Centre);
        PoisonDaggerObject.Fire(Owner, OnCollision, DaggerSpeed, Owner.Centre, rotation);
    }

    private void OnCollision(GameObject gameObject)
    {
        if (gameObject.IsActor())
        {
            Actor actor = gameObject.GetComponent<Actor>();

            if (!actor.Opposes(Owner))
            {
                SuccessFeedbackSubject.Next(false);
                return;
            }

            DealPrimaryDamage(actor);
            ApplySecondaryDamageAsDOT(actor, DotDuration);
            SuccessFeedbackSubject.Next(true);
        }
        else
        {
            SuccessFeedbackSubject.Next(false);
        }
    }
}
