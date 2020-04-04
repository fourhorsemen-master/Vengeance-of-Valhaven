using System;
using UnityEngine;

public class Fireball : InstantCast
{
    private const float FireballSpeed = 5;
    private static readonly Vector3 positionTransform = new Vector3(0, 1.25f, 0);

    public override AbilityReference AbilityReference => AbilityReference.Fireball;

    public override void Cast(AbilityContext context)
    {
        Vector3 position = context.Owner.transform.position + positionTransform;
        Vector3 target = context.TargetPosition;
        Quaternion rotation = Quaternion.LookRotation(target - position);
        FireballObject.Fire(
            context.Owner,
            BuildCollisionCallback(context),
            FireballSpeed,
            position,
            rotation
        );
    }

    private Action<GameObject> BuildCollisionCallback(AbilityContext context)
    {
        return o => OnCollision(o, context);
    }

    private void OnCollision(GameObject gameObject, AbilityContext context)
    {
        if (!gameObject.IsActor()) return;

        Actor actor = gameObject.GetComponent<Actor>();

        if (!actor.Opposes(context.Owner))
        {
            return;
        }

        int strength = context.Owner.GetStat(Stat.Strength);
        actor.ModifyHealth(-strength);
    }
}