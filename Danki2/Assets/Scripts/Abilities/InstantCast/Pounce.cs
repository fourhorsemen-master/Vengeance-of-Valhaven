using System.Collections;
using UnityEngine;

public class Pounce : InstantCast
{
    private float _movementDuration = 0.5f;
    private float _movementSpeedMultiplier = 3f;
    private float _finalRootDuration = 0.3f;
    private float _damageRadius = 2f;

    public static float MinRange = 5f;
    public static float Range => 10f;

    public Pounce(AbilityContext context) : base(context)
    {
    }

    public override void Cast()
    {
        var targetPosition = Context.TargetPosition;
        targetPosition.y = Context.Owner.transform.position.y;

        Context.Owner.LockMovement(
            _movementDuration,
            Context.Owner.GetStat(Stat.Speed) * _movementSpeedMultiplier,
            targetPosition - Context.Owner.transform.position,
            @override: true
        );

        Context.Owner.StartCoroutine(
            DealDamageCoroutine(_movementDuration)
        );
    }

    private IEnumerator DealDamageCoroutine(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        Actor owner = Context.Owner;

        float damage = owner.GetStat(Stat.Strength);

        CollisionTemplateManager.Instance.GetCollidingActors(
            CollisionTemplate.Wedge90,
            _damageRadius,
            owner.transform.position,
            Quaternion.LookRotation(owner.transform.forward)
        ).ForEach(actor =>
        {
            Debug.Log(actor.tag);
            if (owner.Opposes(actor))
            {
                actor.ModifyHealth(-damage);
            }
        });

        owner.Root(_finalRootDuration, owner.transform.forward, true);
    }
}