using System.Collections.Generic;

public class EffectTracker
{
    private List<Effect> _effects;
    private readonly Actor _actor;

    public EffectTracker(Actor actor)
    {
        _effects = new List<Effect>();
        _actor = actor;
    }
        
    public void ProcessEffects()
    {
        _effects = _effects.FindAll(
            effect => {
                effect.Update(_actor);
                return !effect.Expired;
            }    
        );
    }

    public float ProcessStat(Stat stat, float value)
    {
        var processedValue = value;
        _effects.ForEach(e => e.ProcessStat(stat, processedValue));
        return processedValue;
    }

    public void AddEffect(Effect effect)
    {
        _effects.Add(effect);
    }
}
