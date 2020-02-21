using System.Collections;
using UnityEngine;

public class Lunge : InstantCast
{
    private static readonly float _lungeDuration = 0.2f;
    private static readonly float _speedMultiplier = 6f;
    private static readonly float _stunRange = 4f;
    private static readonly float _stunDuration = 2f;

    public Lunge(AbilityContext context) : base(context)
    {
    }

    public override void Cast()
    {
        Actor owner = Context.Owner;

        Vector3 direction = Context.TargetPosition - owner.transform.position;
        direction.y = owner.transform.position.y;

        owner.LockMovement(
            _lungeDuration,
            owner.GetStat(Stat.Speed) * _speedMultiplier,
            direction,
            passThrough: true
        );

        owner.StartCoroutine(Stun(direction, owner));
    }

    private IEnumerator Stun(Vector3 direction, Actor owner)
    {
        yield return new WaitForSeconds(_lungeDuration);

        CollisionTemplateManager.Instance.GetCollidingActors(
            CollisionTemplate.Wedge90,
            _stunRange,
            Context.Owner.transform.position,
            Quaternion.LookRotation(direction)
        ).ForEach(actor =>
        {
            if (owner.Opposes(actor))
            {
                actor.LockMovement(
                    _stunDuration,
                    0,
                    Vector3.zero,
                    @override: true
                );
            }
        });
    }
}
