public class PoisonHandler : ActiveEffectHandler
{
    private const float TickInterval = 2f;
    private const float TickStartDelay = 2f;
    private const int DamagePerTick = 1;

    public PoisonHandler(Actor actor, EffectManager effectManager)
        : base(actor, effectManager, ActiveEffect.Poison, TickInterval, TickStartDelay) {}

    protected override void HandleEffectTicked() => actor.HealthManager.TickDamage(DamagePerTick);
}
