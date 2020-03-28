using System.Collections;
using UnityEngine;

public class Lunge : InstantCast
{
    private static readonly float _lungeDuration = 0.2f;
    private static readonly float _lungeSpeedMultiplier = 6f;
    private static readonly float _lungeDamageMultiplier = 0.5f;
    private static readonly float _stunRange = 2f;
    private static readonly float _stunDuration = 0.5f;

    public Lunge(AbilityContext context) : base(context)
    {
    }

    public override void Cast()
    {
        Actor owner = Context.Owner;
        Vector3 position = owner.transform.position;
        Vector3 target = Context.TargetPosition;

        Vector3 direction = target - position;
        direction.y = position.y;

        owner.LockMovement(
            _lungeDuration,
            owner.GetStat(Stat.Speed) * _lungeSpeedMultiplier,
            direction,
            passThrough: true
        );

        LungeObject.Create(position, Quaternion.LookRotation(target - position));

        owner.WaitAndAct(_lungeDuration, () =>
        {
            float damage = owner.GetStat(Stat.Strength) * _lungeDamageMultiplier;

            CollisionTemplateManager.Instance.GetCollidingActors(
                CollisionTemplate.Wedge90,
                _stunRange,
                owner.transform.position,
                Quaternion.LookRotation(direction)
            ).ForEach(actor =>
            {
                if (owner.Opposes(actor))
                {
                    actor.AddActiveEffect(new Stun(_stunDuration), _stunDuration);
                    actor.ModifyHealth(-damage);
                }
            });
        });
    }
}
