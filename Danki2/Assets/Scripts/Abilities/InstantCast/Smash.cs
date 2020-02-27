using UnityEngine;

public class Smash : InstantCast
{
    private static readonly float _distanceFromCaster = 1f;
    private static readonly float _radius = 1f;
    private static readonly float _damageMultiplier = 2f;

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
        Vector3 center = position + (directionToTarget * _distanceFromCaster);

        float damage = owner.GetStat(Stat.Strength) * _damageMultiplier;

        CollisionTemplateManager.Instance.GetCollidingActors(
            CollisionTemplate.Cylinder,
            _radius,
            center
        ).ForEach(actor =>
        {
            if (owner.Opposes(actor))
            {
                actor.ModifyHealth(-damage);
            }
        });
    }
}
