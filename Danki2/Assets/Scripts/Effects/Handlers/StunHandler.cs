public class StunHandler : IMovementStatusProvider
{
    private readonly Actor actor;

    public StunHandler(Actor actor)
    {
        this.actor = actor;
    }

    public bool Stuns() => actor.EffectManager.HasActiveEffect(ActiveEffect.Stun);
}
