public abstract class StackingEffectHandler : EffectHandler<StackingEffect>
{
    protected sealed override Subject<StackingEffect> EffectAddedSubject => effectManager.StackingEffectAddedSubject;
    protected sealed override Subject<StackingEffect> EffectRemovedSubject => effectManager.StackingEffectRemovedSubject;

    protected StackingEffectHandler(
        Actor actor,
        EffectManager effectManager,
        StackingEffect effectToHandle,
        float tickInterval,
        float tickStartDelay = 0
    ) : base(actor, effectManager, effectToHandle, tickInterval, tickStartDelay) {}
}
