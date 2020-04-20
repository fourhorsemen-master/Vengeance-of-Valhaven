using System;
using UnityEngine;

public class Fireball : InstantCast
{
    private const float FireballSpeed = 5;
    private static readonly Vector3 _positionTransform = new Vector3(0, 1.25f, 0);

    public Fireball(AbilityContext context, Action<bool> completionCallback)
        : base(context, completionCallback)
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
        if (gameObject.IsActor())
        {
            Actor actor = gameObject.GetComponent<Actor>();

            if (!actor.Opposes(Context.Owner))
            {
                isSuccessfulCallback(false);
                return;
            }

            int strength = Context.Owner.GetStat(Stat.Strength);
            actor.ModifyHealth(-strength);

            isSuccessfulCallback(true);
        }
        else
        {
            isSuccessfulCallback(false);
        }
    }
}
