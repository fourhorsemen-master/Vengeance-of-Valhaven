using System;
using UnityEngine;

public class Whirlwind : Channel
{
    private static readonly float _spinRange = 2;
    private static readonly float _spinDpsMultiplier = 0.3f;
    private static readonly float _selfSlowMultiplier = 0.5f;
    private static readonly float _finishRange = 3;
    private static readonly float _finishDamageMultiplier = 1;

    private WhirlwindObject _whirlwindObject;

    private Guid slowEffectId;

    public Whirlwind(AbilityContext context) : base(context) { }

    public override float Duration => 2f;

    public override void Start()
    {
        slowEffectId = Context.Owner.AddPassiveEffect(new Slow(_selfSlowMultiplier));

        Vector3 position = Context.Owner.transform.position;
        Vector3 target = Context.TargetPosition;
        _whirlwindObject = WhirlwindObject.Create(position, Quaternion.LookRotation(target - position));
    }

    public override void Continue()
    {
        AOE(_spinRange, _spinDpsMultiplier * Time.deltaTime);
    }

    public override void Cancel()
    {
        Context.Owner.RemovePassiveEffect(slowEffectId);
        GameObject.Destroy(_whirlwindObject);
    }

    public override void End()
    {
        AOE(_finishRange, _finishDamageMultiplier);
        Context.Owner.RemovePassiveEffect(slowEffectId);
        GameObject.Destroy(_whirlwindObject);
    }

    private void AOE(float radius, float damageMultiplier)
    {
        Actor owner = Context.Owner;
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
