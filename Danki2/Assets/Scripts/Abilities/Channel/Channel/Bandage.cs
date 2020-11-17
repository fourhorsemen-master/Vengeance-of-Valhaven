using System;
using UnityEngine;

[Ability(AbilityReference.Bandage, new []{"Perseverance"})]
public class Bandage : Channel
{
    private const float HealInterval = 1f;
    private const float HealStartDelay = 1f;
    private const float SlowMultiplier = 0.5f;

    private BandageObject bandageObject;

    private Repeater repeater;
    private bool hasHealed = false;

    private Guid slowEffectId;
    
    public override ChannelEffectOnMovement EffectOnMovement => HasBonus("Perseverance")
        ? ChannelEffectOnMovement.None
        : ChannelEffectOnMovement.Stun;

    public Bandage(Actor owner, AbilityData abilityData, string[] availableBonuses, float duration)
        : base(owner, abilityData, availableBonuses, duration)
    {
    }

    public override void Start(Vector3 floorTargetPosition, Vector3 offsetTargetPosition)
    {
        bandageObject = BandageObject.Create(Owner.transform);

        repeater = new Repeater(HealInterval, Heal, HealStartDelay);

        if (HasBonus("Perseverance"))
        {
            Owner.EffectManager.TryAddPassiveEffect(new Slow(SlowMultiplier), out slowEffectId);
        }
    }

    public override void Continue(Vector3 floorTargetPosition, Vector3 offsetTargetPosition)
    {
        repeater.Update();
    }

    public override void Cancel(Vector3 floorTargetPosition, Vector3 offsetTargetPosition) => End();
    
    public override void End(Vector3 floorTargetPosition, Vector3 offsetTargetPosition) => End();

    private void Heal()
    {
        bandageObject.PlaySound();
        CustomCamera.Instance.AddShake(ShakeIntensity.Low);

        base.Heal();

        if (hasHealed) return;
        SuccessFeedbackSubject.Next(true);
        hasHealed = true;
    }

    private void End()
    {
        bandageObject.Destroy();

        if (!hasHealed) SuccessFeedbackSubject.Next(false);

        if (HasBonus("Perseverance"))
        {
            Owner.EffectManager.RemoveEffect(slowEffectId);
        }
    }
}
