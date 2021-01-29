public class SlowHandler : IStatPipe
{
    private const float SlowMultiplier = 0.5f;

    private readonly Actor actor;

    public SlowHandler(Actor actor)
    {
        this.actor = actor;
    }

    public float ProcessStat(Stat stat, float value)
    {
        if (stat == Stat.Speed && actor.EffectManager.HasActiveEffect(ActiveEffect.Slow))
        {
            return value * SlowMultiplier;
        }

        return value;
    }
}
