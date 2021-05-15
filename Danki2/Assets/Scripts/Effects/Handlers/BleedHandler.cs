public class BleedHandler : StackingEffectHandler
{
    private const float TickInterval = 5/3f;
    private const float TickStartDelay = 1f;
    private const int DamagePerStack = 1;

    private const int DeepWoundsMultiplier = 2;
    
    public BleedHandler(Actor actor, EffectManager effectManager)
        : base(actor, effectManager, StackingEffect.Bleed, TickInterval, TickStartDelay) {}

    protected override void HandleEffectTicked()
    {
        int damage = !actor.IsPlayer && ActorCache.Instance.Player.RuneManager.HasRune(Rune.DeepWounds)
            ? DamagePerStack * effectManager.GetStacks(StackingEffect.Bleed) * DeepWoundsMultiplier
            : DamagePerStack * effectManager.GetStacks(StackingEffect.Bleed);

        actor.HealthManager.TickDamage(damage);
    }
}
