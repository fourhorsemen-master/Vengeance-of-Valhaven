public class PassiveSlowHandler : IStatPipe
{
    private const float SpeedMultiplier = 0.5f;
    
    private readonly Actor actor;

    public PassiveSlowHandler(Actor actor)
    {
        this.actor = actor;
    }

    public float ProcessStat(Stat stat, float value)
    {
        return stat == Stat.Speed && actor.EffectManager.HasPassiveEffect(PassiveEffect.Slow)
            ? value * SpeedMultiplier
            : value;
    }
}
