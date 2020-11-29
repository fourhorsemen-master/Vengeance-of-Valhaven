public class BleedHandler : StackingEffectHandler
{
    private const float TickInterval = 1f;
    private const float TickStartDelay = 1f;
    
    public BleedHandler(Actor actor, EffectManager effectManager)
        : base(actor, effectManager, StackingEffect.Bleed, TickInterval, TickStartDelay) {}

    protected override void HandleEffectTicked()
    {
        actor.HealthManager.TickDamage(effectManager.GetStacks(StackingEffect.Bleed));
    }
}
