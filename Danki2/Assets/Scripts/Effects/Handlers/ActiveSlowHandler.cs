public class ActiveSlowHandler : IStatPipe
{
    private const float SlowMultiplier = 0.5f;

    private readonly Actor actor;

    public ActiveSlowHandler(Actor actor)
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
