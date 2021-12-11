using System;

public abstract class EffectHandler<TEffect> where TEffect : Enum
{
    protected readonly Actor actor;
    protected readonly EffectManager effectManager;

    private readonly TEffect effectToHandle;

    private Repeater repeater;

    protected abstract Subject<TEffect> EffectAddedSubject { get; }
    protected abstract Subject<TEffect> EffectRemovedSubject { get; }

    protected EffectHandler(
        Actor actor,
        EffectManager effectManager,
        TEffect effectToHandle,
        float tickInterval = 0,
        float tickStartDelay = 0
    )
    {
        this.actor = actor;
        this.effectManager = effectManager;
        this.effectToHandle = effectToHandle;

        EffectAddedSubject
            .Where(EffectFilter)
            .Subscribe(_ =>
            {
                if (tickInterval > 0)
                {
                    repeater ??= new Repeater(tickInterval, HandleEffectTicked, tickStartDelay);
                }
                HandleEffectAdded();
            });

        EffectRemovedSubject
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

    private bool EffectFilter(TEffect effect) => effect.Equals(effectToHandle);
}
