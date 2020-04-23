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

    private bool hasHitActor = false;
    private WhirlwindObject whirlwindObject;

    private Guid slowEffectId;

    private Repeater repeater;

    public override float Duration => 2f;

    public Whirlwind(AbilityContext context) : base(context)
    {
    }

    public override void Start()
    {
        slowEffectId = Context.Owner.EffectManager.AddPassiveEffect(new Slow(selfSlowMultiplier));
        repeater = new Repeater(spinDamageInterval, () => AOE(spinRange, spinDamageMultiplier), spinDamageStartDelay);

        Vector3 position = Context.Owner.transform.position;
        Vector3 target = Context.TargetPosition;
        whirlwindObject = WhirlwindObject.Create(Context.Owner.transform);
    }

    public override void Continue()
    {
        repeater.Update();
    }

    public override void Cancel()
    {
        if (!this.hasHitActor) SuccessFeedbackSubject.Next(false);

        Context.Owner.EffectManager.RemovePassiveEffect(slowEffectId);
        whirlwindObject.DestroyWhirlwind();
    }

    public override void End()
    {
        AOE(finishRange, finishDamageMultiplier);

        if (!this.hasHitActor) SuccessFeedbackSubject.Next(false);

        Context.Owner.EffectManager.RemovePassiveEffect(slowEffectId);
        whirlwindObject.DestroyWhirlwind();
    }

    private void AOE(float radius, float damageMultiplier)
    {
        Actor owner = Context.Owner;
        int damage = Mathf.CeilToInt(owner.GetStat(Stat.Strength) * damageMultiplier);

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
                this.hasHitActor = true;
            }
        });

        if (this.hasHitActor)
        {
            CustomCamera.Instance.AddShake(8f, 0.05f);
            SuccessFeedbackSubject.Next(true);
        }
    }
}
