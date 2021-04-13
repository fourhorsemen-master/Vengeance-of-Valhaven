public abstract class ActiveEffectHandler : EffectHandler<ActiveEffect>
{
    protected sealed override Subject<ActiveEffect> EffectAddedSubject => effectManager.ActiveEffectAddedSubject;
    protected sealed override Subject<ActiveEffect> EffectRemovedSubject => effectManager.ActiveEffectRemovedSubject;

    protected ActiveEffectHandler(
        Actor actor,
        EffectManager effectManager,
        ActiveEffect effectToHandle,
        float tickInterval,
        float tickStartDelay = 0
    ) : base(actor, effectManager, effectToHandle, tickInterval, tickStartDelay) {}
}
