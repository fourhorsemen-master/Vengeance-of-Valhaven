using System;
using UnityEngine;

[Ability(AbilityReference.Reflect)]
public class Reflect : Channel
{
    private const float Range = 3;
    private const float VisualPositionOffset = 0.7f;

    private ReflectObject reflectObject;

    private Guid damagePipeId;
    private readonly Subject onReflect = new Subject();

    public override ChannelEffectOnMovement EffectOnMovement => ChannelEffectOnMovement.Root;

    public Reflect(AbilityConstructionArgs arguments) : base(arguments) { }

    public override void Start(Vector3 floorTargetPosition, Vector3 offsetTargetPosition)
    {
        reflectObject = ReflectObject.Create(Owner.transform, Owner.Height, onReflect, VisualPositionOffset);
        damagePipeId = Owner.HealthManager.RegisterDamagePipe(DamagePipe);
    }

    public override void Continue(Vector3 floorTargetPosition, Vector3 offsetTargetPosition)
    {
        Owner.MovementManager.LookAt(floorTargetPosition);
    }

    public override void Cancel(Vector3 floorTargetPosition, Vector3 offsetTargetPosition) => Finish();

    public override void End(Vector3 floorTargetPosition, Vector3 offsetTargetPosition) => Finish();

    private bool DamagePipe(DamageData damageData)
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

        if (damageSourceInRange)
        {
            SuccessFeedbackSubject.Next(true);

            onReflect.Next();
            DealPrimaryDamage(damageData.Source, damageData.Damage);
        }

        return !damageSourceInRange;
    }

    private void Finish()
    {
        reflectObject.Destroy();
        Owner.HealthManager.DeregisterDamagePipe(damagePipeId);
        SuccessFeedbackSubject.Next(false);
    }
}
