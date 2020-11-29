public class SlowHandler : IStatPipe
{
    private const float SpeedReductionPerStack = 0.1f;
    
    private readonly Actor actor;

    public SlowHandler(Actor actor, StatsManager statsManager)
    {
        this.actor = actor;

        actor.EffectManager.StackingEffectAddedSubject
            .Where(effect => effect == StackingEffect.Slow)
            .Subscribe(_ => statsManager.ClearCache());

        actor.EffectManager.StackingEffectRemovedSubject
            .Where(effect => effect == StackingEffect.Slow)
            .Subscribe(_ => statsManager.ClearCache());
    }

    public float ProcessStat(Stat stat, float value)
    {
        return stat == Stat.Speed
            ? value * (1 - SpeedReductionPerStack * actor.EffectManager.GetStacks(StackingEffect.Slow))
            : value;
    }
}
