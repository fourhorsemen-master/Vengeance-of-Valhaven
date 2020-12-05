public class StackingSlowHandler : IStatPipe
{
    private const float SpeedReductionPerStack = 0.1f;
    
    private readonly Actor actor;

    public StackingSlowHandler(Actor actor)
    {
        this.actor = actor;
    }

    public float ProcessStat(Stat stat, float value)
    {
        return stat == Stat.Speed
            ? value * (1 - SpeedReductionPerStack * actor.EffectManager.GetStacks(StackingEffect.Slow))
            : value;
    }
}
