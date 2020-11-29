public class BleedHandler : StackingEffectHandler
{
    private const float TickInterval = 1f;
    private const float TickStartDelay = 1f;
    private const int DamagePerStack = 2;
    
    public BleedHandler(Actor actor, EffectManager effectManager)
        : base(actor, effectManager, StackingEffect.Bleed, TickInterval, TickStartDelay) {}

    protected override void HandleEffectTicked()
    {
        int damage = DamagePerStack * effectManager.GetStacks(StackingEffect.Bleed);
        actor.HealthManager.TickDamage(damage);
    }
}
