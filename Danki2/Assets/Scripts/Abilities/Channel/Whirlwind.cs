using System;
using UnityEngine;

public class Whirlwind : Channel
{
    private const float spinRange = 2;
    private const float spinDamageMultiplier = 0.3f;
    private const float spinDamageInterval = 0.35f;
    private const float spinDamageStartDelay = 0.1f;
    private const float selfSlowMultiplier = 0.5f;
    private const float finishRange = 3;
    private const float finishDamageMultiplier = 1;

    private WhirlwindObject whirlwindObject;

    private Guid slowEffectId;

    private Repeater repeater;

    public override AbilityReference AbilityReference => AbilityReference.Whirlwind;
    public override float Duration => 2f;

    public override void Start(AbilityContext context)
    {
        slowEffectId = context.Owner.AddPassiveEffect(new Slow(selfSlowMultiplier));
        repeater = new Repeater(spinDamageInterval, () => AOE(spinRange, spinDamageMultiplier, context.Owner), spinDamageStartDelay);

        Vector3 position = context.Owner.transform.position;
        Vector3 target = context.TargetPosition;
        whirlwindObject = WhirlwindObject.Create(position, Quaternion.LookRotation(target - position));
    }

    public override void Continue(AbilityContext context)
    {
        repeater.Update();
    }

    public override void Cancel(AbilityContext context)
    {
        context.Owner.RemovePassiveEffect(slowEffectId);
        GameObject.Destroy(whirlwindObject);
    }

    public override void End(AbilityContext context)
    {
        AOE(finishRange, finishDamageMultiplier, context.Owner);
        context.Owner.RemovePassiveEffect(slowEffectId);
        GameObject.Destroy(whirlwindObject);
    }

    private void AOE(float radius, float damageMultiplier, Actor owner)
    {
        float damage = owner.GetStat(Stat.Strength) * damageMultiplier;

        CollisionTemplateManager.Instance.GetCollidingActors(
            CollisionTemplate.Cylinder,
            radius,
            owner.transform.position,
            Quaternion.identity
        ).ForEach(actor =>
        {
            if (actor.Opposes(owner))
            {
                actor.ModifyHealth(-damage);
            }
        });
    }
}
