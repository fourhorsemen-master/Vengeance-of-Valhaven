using UnityEngine;

public class Fireball : InstantCast
{
    private const float FireballSpeed = 5;
    private static readonly Vector3 _positionTransform = new Vector3(0, 1.25f, 0);

    public Fireball(AbilityContext context) : base(context)
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
        CustomCamera.Instance.AddShake(12f, 0.2f);

        if (gameObject.IsActor())
        {
            Actor actor = gameObject.GetComponent<Actor>();

            if (!actor.Opposes(Context.Owner))
            {
                SuccessFeedbackSubject.Next(false);
                return;
            }

            int strength = Context.Owner.GetStat(Stat.Strength);
            actor.ModifyHealth(-strength);

            SuccessFeedbackSubject.Next(true);
        }
        else
        {
            SuccessFeedbackSubject.Next(false);
        }
    }
}
