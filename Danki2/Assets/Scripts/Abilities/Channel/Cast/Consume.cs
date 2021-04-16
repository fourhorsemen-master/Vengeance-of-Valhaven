using UnityEngine;

[Ability(AbilityReference.Consume, new []{ "Vampiric" })]
public class Consume : Cast
{
    private const int DamagePerStack = 2;
    private const float PauseDuration = 0.3f;
    private const int HealPerStack = 1;
    
    private bool HasVampiric => HasBonus("Vampiric");
    
    public Consume(AbilityConstructionArgs arguments) : base(arguments) {}

    public override void End(Vector3 floorTargetPosition, Vector3 offsetTargetPosition)
    {
        int stacksConsumed = 0;

        ActorCache.Instance.Cache
            .Where(actorCacheItem => Owner.Opposes(actorCacheItem.Actor))
            .ForEach(actorCacheItem =>
            {
                int stacks = actorCacheItem.Actor.EffectManager.GetStacks(StackingEffect.Bleed);
                stacksConsumed += stacks;
                actorCacheItem.Actor.EffectManager.RemoveStackingEffect(StackingEffect.Bleed);
                actorCacheItem.Actor.HealthManager.ReceiveDamage(stacks * DamagePerStack, Owner);
            });

        if (stacksConsumed > 0)
        {
            SuccessFeedbackSubject.Next(true);
            CustomCamera.Instance.AddShake(ShakeIntensity.High);
            if (HasVampiric) Owner.HealthManager.ReceiveHeal(stacksConsumed * HealPerStack);
        }

        SuccessFeedbackSubject.Next(false);
        Owner.MovementManager.Pause(PauseDuration);
    }
}
