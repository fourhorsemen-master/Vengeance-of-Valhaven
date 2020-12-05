public class VulnerableHandler : IStatPipe
{
    private const int DefenceReductionPerStack = 1;

    private readonly Actor actor;

    public VulnerableHandler(Actor actor)
    {
        this.actor = actor;
    }

    public float ProcessStat(Stat stat, float value)
    {
        return stat == Stat.Defence
            ? value - (DefenceReductionPerStack * actor.EffectManager.GetStacks(StackingEffect.Vulnerable))
            : value;
    }
}
