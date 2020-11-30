public class PassiveSlowHandler : IStatPipe
{
    private const float SpeedMultiplier = 0.5f;
    
    private readonly Actor actor;

    public PassiveSlowHandler(Actor actor, StatsManager statsManager)
    {
        this.actor = actor;

        actor.EffectManager.PassiveEffectAddedSubject
            .Where(passiveEffectData => passiveEffectData.Effect == PassiveEffect.Slow)
            .Subscribe(_ => statsManager.ClearCache());

        actor.EffectManager.PassiveEffectRemovedSubject
            .Where(passiveEffectData => passiveEffectData.Effect == PassiveEffect.Slow)
            .Subscribe(_ => statsManager.ClearCache());
    }

    public float ProcessStat(Stat stat, float value)
    {
        return stat == Stat.Speed && actor.EffectManager.HasPassiveEffect(PassiveEffect.Slow)
            ? value * SpeedMultiplier
            : value;
    }
}
