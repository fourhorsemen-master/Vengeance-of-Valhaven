using System.Collections.Generic;

public class EffectTracker : StatPipe
{
    private List<Effect> _effects;
    private readonly Actor _actor;
    private readonly StatsManager _statsManager;

    public EffectTracker(Actor actor, StatsManager statsManager)
    {
        _effects = new List<Effect>();
        _actor = actor;
        _statsManager = statsManager;
        _statsManager.RegisterPipe(this);
    }
        
    public void ProcessEffects()
    {
        var someExpired = false;

        _effects = _effects.FindAll(
            effect => {
                effect.Update(_actor);
                someExpired = effect.Expired || someExpired;                
                return !effect.Expired;
            }
        );

        if (someExpired) _statsManager.ClearCache();
    }

    public float ProcessStat(Stat stat, float value)
    {
        var processedValue = value;
        _effects.ForEach(e => processedValue = e.ProcessStat(stat, processedValue));
        return processedValue;
    }

    public void AddEffect(Effect effect)
    {
        _effects.Add(effect);
        _statsManager.ClearCache();
    }
}
