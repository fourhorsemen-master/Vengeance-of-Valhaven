using UnityEngine;

public class Fireball : InstantCast
{
    public static readonly AbilityData BaseAbilityData = new AbilityData(0, 0, 0);

    private const int Damage = 5;
    private const float FireballSpeed = 5;
    private static readonly Vector3 _positionTransform = new Vector3(0, 1.25f, 0);

    public Fireball(AbilityContext context, AbilityData abilityData) : base(context, abilityData)
    {
    }

    public override void Cast()
    {
        Vector3 position = Context.Owner.transform.position + _positionTransform;
        Vector3 target = Context.TargetPosition;
        Quaternion rotation = Quaternion.LookRotation(target - position);
        FireballObject.Fire(Context.Owner, OnCollision, FireballSpeed, position, rotation);
    }

    private void OnCollision(GameObject gameObject)
    {
        CustomCamera.Instance.AddShake(ShakeIntensity.High);

        if (gameObject.IsActor())
        {
            Actor actor = gameObject.GetComponent<Actor>();

            if (!actor.Opposes(Context.Owner))
            {
                SuccessFeedbackSubject.Next(false);
                return;
            }

            actor.ModifyHealth(-Damage);

            SuccessFeedbackSubject.Next(true);
        }
        else
        {
            SuccessFeedbackSubject.Next(false);
        }
    }
}
