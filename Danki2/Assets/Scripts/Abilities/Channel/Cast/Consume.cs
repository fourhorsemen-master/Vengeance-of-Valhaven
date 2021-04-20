using UnityEngine;

[Ability(AbilityReference.Consume, new []{ "Vampiric" })]
public class Consume : Cast
{
    private const int DamagePerStack = 3;
    private const float PauseDuration = 0.3f;
    private const int HealPerStack = 1;
    
    private readonly Subject onCastSuccessful = new Subject();
    private readonly Subject onCastFailed = new Subject();

    private bool HasVampiric => HasBonus("Vampiric");
    
    public Consume(AbilityConstructionArgs arguments) : base(arguments) {}

    protected override void Start() => ConsumeObject.Create(Owner.transform.position, onCastSuccessful, onCastFailed);

    protected override void Cancel() => onCastFailed.Next();

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
            onCastSuccessful.Next();
            CustomCamera.Instance.AddShake(ShakeIntensity.Medium);
            if (HasVampiric) Owner.HealthManager.ReceiveHeal(stacksConsumed * HealPerStack);
        }
        else
        {
            SuccessFeedbackSubject.Next(false);
            onCastFailed.Next();
        }

        Owner.MovementManager.Pause(PauseDuration);
    }
}
