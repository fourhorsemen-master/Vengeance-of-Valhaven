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
    
    public override float Duration => 5f;
    
    public override ChannelEffectOnMovement EffectOnMovement => HasBonus("Perseverance")
        ? ChannelEffectOnMovement.None
        : ChannelEffectOnMovement.Stun;

    public Bandage(Actor owner, AbilityData abilityData, string[] availableBonuses) : base(owner, abilityData, availableBonuses)
    {
    }

    public override void Start(Vector3 target)
    {
        bandageObject = BandageObject.Create(Owner.transform);
        Owner.DeathSubject.Subscribe(bandageObject.Destroy);

        repeater = new Repeater(HealInterval, Heal, HealStartDelay);

        if (HasBonus("Perseverance"))
        {
            Owner.EffectManager.TryAddPassiveEffect(new Slow(SlowMultiplier), out slowEffectId);
        }
    }

    public override void Continue(Vector3 target)
    {
        repeater.Update();
    }

    public override void Cancel(Vector3 target) => End();
    
    public override void End(Vector3 target) => End();

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
            Owner.EffectManager.RemovePassiveEffect(slowEffectId);
        }
    }
}
