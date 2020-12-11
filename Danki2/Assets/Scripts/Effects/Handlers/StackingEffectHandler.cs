public abstract class StackingEffectHandler
{
    protected readonly Actor actor;
    protected readonly EffectManager effectManager;

    private readonly StackingEffect effectToHandle;

    private Repeater repeater;

    protected StackingEffectHandler(
        Actor actor,
        EffectManager effectManager,
        StackingEffect effectToHandle,
        float tickInterval,
        float tickStartDelay = 0
    )
    {
        this.actor = actor;
        this.effectManager = effectManager;
        this.effectToHandle = effectToHandle;

        effectManager.StackingEffectAddedSubject
            .Where(EffectFilter)
            .Subscribe(_ =>
            {
                repeater = repeater ?? new Repeater(tickInterval, HandleEffectTicked, tickStartDelay);
                HandleEffectAdded();
            });

        effectManager.StackingEffectRemovedSubject
            .Where(EffectFilter)
            .Subscribe(_ =>
            {
                repeater = null;
                HandleEffectRemoved();
            });
    }

    public void Update() => repeater?.Update();

    protected virtual void HandleEffectAdded() {}
    
    protected virtual void HandleEffectRemoved() {}
    
    protected virtual void HandleEffectTicked() {}

    private bool EffectFilter(StackingEffect effect) => effect == effectToHandle;
}
