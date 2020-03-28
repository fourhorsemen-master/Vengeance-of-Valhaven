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
        Actor owner = Context.Owner;
        Vector3 position = owner.transform.position;
        Vector3 target = Context.TargetPosition;
        target.y = position.y;

        owner.LockMovement(
            _movementDuration,
            owner.GetStat(Stat.Speed) * _movementSpeedMultiplier,
            target - owner.transform.position,
            @override: true
        );

        PounceObject.Create(position, Quaternion.LookRotation(target - position));

        owner.WaitAndAct(_movementDuration, () =>
        {
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
        });
    }
}