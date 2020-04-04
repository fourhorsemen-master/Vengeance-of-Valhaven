using UnityEngine;

public class Smash : InstantCast
{
    private const float DistanceFromCaster = 1f;
    private const float Radius = 1f;
    private const float DamageMultiplier = 2f;

    public override AbilityReference AbilityReference => AbilityReference.Smash;

    public override void Cast(AbilityContext context)
    {
        Actor owner = context.Owner;

        Vector3 position = owner.transform.position;
        Vector3 target = context.TargetPosition;
        target.y = 0;

        Vector3 directionToTarget = target == position ? Vector3.right : (target - position).normalized;
        Vector3 center = position + (directionToTarget * DistanceFromCaster);

        float damage = owner.GetStat(Stat.Strength) * DamageMultiplier;

        CollisionTemplateManager.Instance.GetCollidingActors(
            CollisionTemplate.Cylinder,
            Radius,
            center
        ).ForEach(actor =>
        {
            if (owner.Opposes(actor))
            {
                actor.ModifyHealth(-damage);
            }
        });

        SmashObject.Create(position, Quaternion.LookRotation(target - position));
    }
}
