using UnityEngine;

[Ability(AbilityReference.Fireball)]
public class Fireball : InstantCast
{
    private const float FireballSpeed = 5;
    private static readonly Vector3 _positionTransform = new Vector3(0, 1.25f, 0);

    public Fireball(Actor owner, AbilityData abilityData) : base(owner, abilityData)
    {
    }

    public override void Cast(Vector3 target)
    {
        Vector3 position = Owner.transform.position + _positionTransform;
        Quaternion rotation = Quaternion.LookRotation(target - position);
        FireballObject.Fire(Owner, OnCollision, FireballSpeed, position, rotation);
    }

    private void OnCollision(GameObject gameObject)
    {
        CustomCamera.Instance.AddShake(ShakeIntensity.High);

        if (gameObject.IsActor())
        {
            Actor actor = gameObject.GetComponent<Actor>();

            if (!actor.Opposes(Owner))
            {
                SuccessFeedbackSubject.Next(false);
                return;
            }

            DealPrimaryDamage(actor);
            SuccessFeedbackSubject.Next(true);
        }
        else
        {
            SuccessFeedbackSubject.Next(false);
        }
    }
}
