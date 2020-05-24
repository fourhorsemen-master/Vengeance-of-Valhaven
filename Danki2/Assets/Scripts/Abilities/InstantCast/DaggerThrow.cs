using UnityEngine;

[Ability(AbilityReference.DaggerThrow)]
public class DaggerThrow : InstantCast
{
    private const float DaggerSpeed = 20f;
    private const float DotDuration = 3f;
    private static readonly Vector3 positionTransform = new Vector3(0, 1.25f, 0);

    public DaggerThrow(Actor owner, AbilityData abilityData, string[] availableBonuses) : base(owner, abilityData, availableBonuses)
    {
    }

    public override void Cast(Vector3 target)
    {
        Vector3 position = Owner.transform.position + positionTransform;
        Quaternion rotation = Quaternion.LookRotation(target - position);
        DaggerObject.Fire(Owner, OnCollision, DaggerSpeed, position, rotation);
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
