using UnityEngine;

public class Pounce : InstantCast
{
    // The ai casting this ability should determine cast range
    private const float MovementDuration = 0.5f;
    private const float MovementSpeedMultiplier = 3f;
    private const float DamageRadius = 2f;
    private const float PauseDuration = 0.3f;

    public Pounce(AbilityContext context) : base(context)
    {
    }

    public override void Cast()
    {
        Actor owner = Context.Owner;
        Vector3 position = owner.transform.position;
        Vector3 target = Context.TargetPosition;
        target.y = position.y;
        Vector3 direction = target - position;

        owner.MovementManager.LockMovement(
            MovementDuration,
            owner.GetStat(Stat.Speed) * MovementSpeedMultiplier,
            direction,
            direction
        );

        PounceObject.Create(position, Quaternion.LookRotation(target - position));

        owner.WaitAndAct(MovementDuration, () =>
        {
            int damage = Mathf.CeilToInt(owner.GetStat(Stat.Strength));

            CollisionTemplateManager.Instance.GetCollidingActors(
                CollisionTemplate.Wedge90,
                DamageRadius,
                owner.transform.position,
                Quaternion.LookRotation(owner.transform.forward)
            ).ForEach(actor =>
            {
                if (owner.Opposes(actor))
                {
                    actor.ModifyHealth(-damage);
                }
            });

            owner.MovementManager.Stun(PauseDuration);
        });
    }
}