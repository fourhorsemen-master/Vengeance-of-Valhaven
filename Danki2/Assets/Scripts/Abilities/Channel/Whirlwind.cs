using System;
using UnityEngine;

public class Whirlwind : Channel
{
    private static readonly float spinRange = 2;
    private static readonly float spinDpsMultiplier = 0.3f;
    private static readonly float slowMultiplier = 0.5f;
    private static readonly float finishRange = 3;
    private static readonly float finishDamageMultiplier = 1;

    private Guid slowEffectId;

    public Whirlwind(AbilityContext context) : base(context) { }

    public override float Duration => 3f;

    public override void Start()
    {
        AOE(spinRange, spinDpsMultiplier * Time.deltaTime);
        slowEffectId = Context.Owner.AddPassiveEffect(new Slow(slowMultiplier));
    }

    public override void Continue()
    {
        AOE(spinRange, spinDpsMultiplier * Time.deltaTime);
    }

    public override void Cancel()
    {
        Context.Owner.RemovePassiveEffect(slowEffectId);
    }

    public override void End()
    {
        AOE(finishRange, finishDamageMultiplier);
        Context.Owner.RemovePassiveEffect(slowEffectId);
    }

    private void AOE(float size, float damageMultiplier)
    {
        Actor owner = Context.Owner;
        float damage = owner.GetStat(Stat.Strength) * damageMultiplier;

        CollisionTemplateManager.Instance.GetCollidingActors(
            CollisionTemplate.Cylinder,
            size,
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
