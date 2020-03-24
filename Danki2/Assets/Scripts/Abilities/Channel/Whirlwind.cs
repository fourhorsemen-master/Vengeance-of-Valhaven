using System;
using UnityEngine;

public class Whirlwind : Channel
{
    private static readonly float _spinRange = 2;
    private static readonly float _spinDamageMultiplier = 0.3f;
    private static readonly float _selfSlowMultiplier = 0.5f;
    private static readonly float _finishRange = 3;
    private static readonly float _finishDamageMultiplier = 1;
    private static readonly float _damageTickInterval = 0.5f;

    private WhirlwindObject _whirlwindObject;

    private Guid slowEffectId;

    private Repeater repeater;

    public Whirlwind(AbilityContext context) : base(context) { }

    public override float Duration => 2f;

    public override void Start()
    {
        slowEffectId = Context.Owner.AddPassiveEffect(new Slow(_selfSlowMultiplier));
        repeater = new Repeater(_damageTickInterval, () => AOE(_spinRange, _spinDamageMultiplier));

        Vector3 position = Context.Owner.transform.position;
        Vector3 target = Context.TargetPosition;
        _whirlwindObject = WhirlwindObject.Create(position, Quaternion.LookRotation(target - position));
    }

    public override void Continue()
    {
        repeater.Update();
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

public class Repeater
{
    private readonly float interval;
    private readonly Action action;

    private float currentTime = 0;

    public Repeater(float interval, Action action, bool runOnStart = true)
    {
        this.interval = interval;
        this.action = action;

        if (runOnStart) action.Invoke();
    }

    public void Update()
    {
        while (currentTime >= interval)
        {
            action.Invoke();
            currentTime -= interval;
        }

        currentTime += Time.deltaTime;
    }
}
