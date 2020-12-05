public class PoisonHandler : StackingEffectHandler
{
    private const float TickInterval = 0.5f;
    private const float TickStartDelay = 0.5f;
    private const int DamagePerStack = 1;
    
    public PoisonHandler(Actor actor, EffectManager effectManager)
        : base(actor, effectManager, StackingEffect.Poison, TickInterval, TickStartDelay) {}

    protected override void HandleEffectTicked()
    {
        int damage = DamagePerStack * effectManager.GetStacks(StackingEffect.Poison);
        actor.HealthManager.TickDamage(damage);
    }
}
