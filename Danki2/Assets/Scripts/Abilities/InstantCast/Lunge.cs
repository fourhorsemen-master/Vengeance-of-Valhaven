using System.Collections;
using UnityEngine;

public class Lunge : InstantCast
{
    private static readonly float _lungeDuration = 0.2f;
    private static readonly float _lungeSpeedMultiplier = 6f;
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
            owner.GetStat(Stat.Speed) * _lungeSpeedMultiplier,
            direction,
            passThrough: true
        );

        owner.StartCoroutine(StunAfter(direction, owner));
    }

    private IEnumerator StunAfter(Vector3 direction, Actor owner)
    {
        yield return new WaitForSeconds(_lungeDuration);

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
            }
        });
    }
}
