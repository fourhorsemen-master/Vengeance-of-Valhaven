using UnityEngine;

public class Slash : InstantCast
{
    private static readonly float range = 3;
    private static readonly float damageMultiplyer = 1.5f;

    public Slash(AbilityContext context) : base(context)
    {
    }

    public override void Cast()
    {
        Actor owner = Context.Owner;
        Vector3 position = owner.transform.position;
        Vector3 target = Context.TargetPosition;

        CollisionTemplateManager.Instance.GetCollidingActors(
            CollisionTemplate.Wedge90,
            range,
            position,
            Quaternion.FromToRotation(position, target)
        ).ForEach(actor =>
        {
            if (true) // TODO: Should check Context.Owner.opposes(actor)...
            {
                float damage = owner.GetStat(Stat.Strength) * damageMultiplyer;
                actor.ModifyHealth(-damage);
            }
        });
    }
}
