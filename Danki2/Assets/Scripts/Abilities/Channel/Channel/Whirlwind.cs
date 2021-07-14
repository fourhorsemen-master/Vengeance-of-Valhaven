using System;
using UnityEngine;

[Ability(AbilityReference.Whirlwind, new[]{"Cross-Step"})]
public class Whirlwind : Channel
{
    private const float spinRange = 2;
    private const float spinDamageInterval = 0.35f;
    private const float spinDamageStartDelay = 0.1f;

    private WhirlwindObject whirlwindObject;

    private bool slowEffectAdded = false;
    private Guid slowEffectId;

    private Repeater repeater;

    public override ChannelEffectOnMovement EffectOnMovement => ChannelEffectOnMovement.None;

    public Whirlwind(AbilityConstructionArgs arguments) : base(arguments) { }

    public override void Start(Vector3 floorTargetPosition, Vector3 offsetTargetPosition)
    {
        repeater = new Repeater(spinDamageInterval, () => AOE(spinRange, a => DealPrimaryDamage(a)), spinDamageStartDelay);

        if (!HasBonus("Cross-Step"))
        {
            slowEffectAdded = Owner.EffectManager.TryAddPassiveEffect(PassiveEffect.Slow, out slowEffectId);
        }

        whirlwindObject = WhirlwindObject.Create(Owner.transform);
    }

    public override void Continue(Vector3 floorTargetPosition, Vector3 offsetTargetPosition)
    {
        repeater.Update();
    }

    public override void Cancel(Vector3 floorTargetPosition, Vector3 offsetTargetPosition) => End();

    public override void End(Vector3 floorTargetPosition, Vector3 offsetTargetPosition) => End();

    private void End()
    {
        if(slowEffectAdded) Owner.EffectManager.RemovePassiveEffect(slowEffectId);

        whirlwindObject.DissipateAndDestroy();
    }

    private void AOE(float radius, Action<Actor> damageAction)
    {
        bool actorsHit = false;

        TemplateCollision(
            CollisionTemplateShape.Cylinder,
            radius,
            Owner.CollisionTemplateSource,
            Quaternion.identity,
            actor =>
            {
                damageAction(actor);
                actorsHit = true;
            },
            CollisionSoundLevel.Low
        );

        if (actorsHit)
        {
            CustomCamera.Instance.AddShake(ShakeIntensity.Low);
        }
    }
}
