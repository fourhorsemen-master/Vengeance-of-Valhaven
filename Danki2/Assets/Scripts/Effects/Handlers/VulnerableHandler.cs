public class VulnerableHandler : IStatPipe
{
    private const int DefenceReductionPerStack = 1;

    private readonly Actor actor;

    public VulnerableHandler(Actor actor, StatsManager statsManager)
    {
        this.actor = actor;

        actor.EffectManager.StackingEffectAddedSubject
            .Where(effect => effect == StackingEffect.Vulnerable)
            .Subscribe(_ => statsManager.ClearCache());

        actor.EffectManager.StackingEffectRemovedSubject
            .Where(effect => effect == StackingEffect.Vulnerable)
            .Subscribe(_ => statsManager.ClearCache());
    }

    public float ProcessStat(Stat stat, float value)
    {
        return stat == Stat.Defence
            ? value - (DefenceReductionPerStack * actor.EffectManager.GetStacks(StackingEffect.Vulnerable))
            : value;
    }
}
