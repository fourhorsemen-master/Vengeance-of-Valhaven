using System;
using UnityEngine;

[Ability(AbilityReference.Blink)]
public class Blink : Cast
{
    private Guid blockEffectId;

    public Blink(AbilityConstructionArgs arguments) : base(arguments) {}

    protected override void Start()
    {
        Owner.EffectManager.TryAddPassiveEffect(PassiveEffect.Block, out blockEffectId);
    }

    protected override void Cancel()
    {
        SuccessFeedbackSubject.Next(false);
        Owner.EffectManager.RemovePassiveEffect(blockEffectId);
    }

    public override void End(Vector3 floorTargetPosition, Vector3 offsetTargetPosition)
    {
        SuccessFeedbackSubject.Next(true);
        Owner.EffectManager.RemovePassiveEffect(blockEffectId);

        Owner.transform.position = floorTargetPosition;

        CustomCamera.Instance.AddShake(ShakeIntensity.Low);
    }
}
