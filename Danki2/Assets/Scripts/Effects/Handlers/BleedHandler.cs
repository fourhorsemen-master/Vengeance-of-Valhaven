public class BleedHandler : StackingEffectHandler
{
    private const float TickInterval = 1f;
    private const float TickStartDelay = 1f;
    private const int DamagePerStack = 1;
    private const int DeepWoundsMultiplier = 3;

    public BleedHandler(Actor actor, EffectManager effectManager)
        : base(actor, effectManager, StackingEffect.Bleed, TickInterval, TickStartDelay) {}

    protected override void HandleEffectTicked()
    {
        int stacks = effectManager.GetStacks(StackingEffect.Bleed);

        if (stacks == 0) return;

        int damage = stacks * DamagePerStack;

        if (RoomManager.Instance.Player.RuneManager.HasRune(Rune.DeepWounds) && actor.Type != ActorType.Player)
        {
            damage *= DeepWoundsMultiplier;
        }

        actor.HealthManager.TickDamage(damage);
    }
}
