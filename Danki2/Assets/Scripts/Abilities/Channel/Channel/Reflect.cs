using System;
using System.Collections.Generic;
using UnityEngine;

[Ability(AbilityReference.Reflect)]
public class Reflect : Channel
{
    private const float Range = 3;
    private const float VisualPositionOffset = 0.7f;

    private ReflectObject reflectObject;

    private Subscription<DamageData> damageSubscription;
    private readonly Subject onReflect = new Subject();
    private bool receivedDamage = false;
    private Guid effectId;

    public override ChannelEffectOnMovement EffectOnMovement => ChannelEffectOnMovement.Root;

    public Reflect(AbilityConstructionArgs arguments) : base(arguments) { }

    public override void Start(Vector3 floorTargetPosition, Vector3 offsetTargetPosition)
    {
        reflectObject = ReflectObject.Create(Owner.transform, Owner.Height, onReflect, VisualPositionOffset);
        Owner.EffectManager.TryAddPassiveEffect(PassiveEffect.Block, out effectId);
        damageSubscription = Owner.HealthManager.UnmodifiedDamageSubject.Subscribe(HandleIncomingDamage);
    }

    public override void Continue(Vector3 floorTargetPosition, Vector3 offsetTargetPosition)
    {
        Owner.MovementManager.LookAt(floorTargetPosition);
    }

    public override void Cancel(Vector3 floorTargetPosition, Vector3 offsetTargetPosition) => Finish();

    public override void End(Vector3 floorTargetPosition, Vector3 offsetTargetPosition) => Finish();

    private void HandleIncomingDamage(DamageData damageData)
    {
        bool damageSourceInRange = false;

        TemplateCollision(
            CollisionTemplateShape.Wedge90,
            Range,
            Owner.transform.position,
            Owner.transform.rotation,
            actor => {
                if (actor == damageData.Source) damageSourceInRange = true;
            }
        );

        if (!damageSourceInRange) return;

        if (!receivedDamage) SuccessFeedbackSubject.Next(true);
        receivedDamage = true;

        onReflect.Next();
        DealPrimaryDamage(damageData.Source, damageData.Damage);
    }

    private void Finish()
    {
        reflectObject.Destroy();
        Owner.EffectManager.RemovePassiveEffect(effectId);
        damageSubscription.Unsubscribe();
        if (!receivedDamage) SuccessFeedbackSubject.Next(false);
    }
}
