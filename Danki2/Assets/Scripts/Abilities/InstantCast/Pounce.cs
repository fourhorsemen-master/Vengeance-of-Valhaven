using System.Collections;
using UnityEngine;

public class Pounce : InstantCast
{
    // The ai casting this ability should determine cast range
    private float _movementDuration = 0.5f;
    private float _movementSpeedMultiplier = 3f;
    private float _finalRootDuration = 0.3f;
    private float _damageRadius = 2f;

    public Pounce(AbilityContext context) : base(context)
    {
    }

    public override void Cast()
    {
        Vector3 targetPosition = Context.TargetPosition;
        targetPosition.y = Context.Owner.transform.position.y;

        Context.Owner.LockMovement(
            _movementDuration,
            Context.Owner.GetStat(Stat.Speed) * _movementSpeedMultiplier,
            targetPosition - Context.Owner.transform.position,
            @override: true
        );

        Context.Owner.StartCoroutine(
            DealDamageCoroutine()
        );
    }

    private IEnumerator DealDamageCoroutine()
    {
        yield return new WaitForSeconds(_movementDuration);

        Actor owner = Context.Owner;

        float damage = owner.GetStat(Stat.Strength);

        CollisionTemplateManager.Instance.GetCollidingActors(
            CollisionTemplate.Wedge90,
            _damageRadius,
            owner.transform.position,
            Quaternion.LookRotation(owner.transform.forward)
        ).ForEach(actor =>
        {
            if (owner.Opposes(actor))
            {
                actor.ModifyHealth(-damage);
            }
        });

        owner.Root(_finalRootDuration, owner.transform.forward, true);
    }
}