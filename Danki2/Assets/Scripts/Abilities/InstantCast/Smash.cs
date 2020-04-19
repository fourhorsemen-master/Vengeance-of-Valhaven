using UnityEngine;

public class Smash : InstantCast
{
    private const float DistanceFromCaster = 1f;
    private const float Radius = 1f;
    private const float DamageMultiplier = 2f;

    public Smash(AbilityContext context) : base(context)
    {
    }

    public override void Cast()
    {
        Actor owner = Context.Owner;

        Vector3 position = owner.transform.position;
        Vector3 target = Context.TargetPosition;
        target.y = 0;

        Vector3 directionToTarget = target == position ? Vector3.right : (target - position).normalized;
        Vector3 center = position + (directionToTarget * DistanceFromCaster);

        int damage = Mathf.CeilToInt(owner.GetStat(Stat.Strength) * DamageMultiplier);

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
