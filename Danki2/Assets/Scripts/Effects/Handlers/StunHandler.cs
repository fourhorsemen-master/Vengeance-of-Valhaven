public class StunHandler : IMovementStatusProvider
{
    private readonly Actor actor;

    public StunHandler(Actor actor)
    {
        this.actor = actor;

        actor.EffectManager.ActiveEffectAddedSubject
            .Where(e => e == ActiveEffect.Stun)
            .Subscribe(_ => actor.InterruptionManager.Interrupt(InterruptionType.Hard));
    }

    public bool Stuns() => actor.EffectManager.HasActiveEffect(ActiveEffect.Stun);

    public bool Roots() => false;
}
