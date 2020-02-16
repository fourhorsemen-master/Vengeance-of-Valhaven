using System;
using System.Collections.Generic;
using UnityEngine;

public class EffectTracker : StatPipe
{
    private List<EffectWithDuration> _activeEffects;
    private Dictionary<Guid, Effect> _passiveEffects;
    private readonly Actor _actor;
    private readonly StatsManager _statsManager;

    public EffectTracker(Actor actor, StatsManager statsManager)
    {
        _activeEffects = new List<EffectWithDuration>();
        _passiveEffects = new Dictionary<Guid, Effect>();
        _actor = actor;
        _statsManager = statsManager;
        _statsManager.RegisterPipe(this);
    }
        
    public void ProcessEffects()
    {
        ProcessActiveEffects();
        ProcessPassiveEffects();
    }

    private void ProcessActiveEffects()
    {
        var someExpired = false;

        _activeEffects = _activeEffects.FindAll(activeEffect =>
        {
            activeEffect.Effect.Update(_actor);
            activeEffect.RemainingDuration -= Time.deltaTime;
            if (activeEffect.RemainingDuration <= 0)
            {
                activeEffect.Effect.Finish(_actor);
                someExpired = true;
                return false;
            }
            return true;
        });

        if (someExpired) _statsManager.ClearCache();
    }

    private void ProcessPassiveEffects()
    {
        foreach (Effect effect in _passiveEffects.Values)
        {
            effect.Update(_actor);
        }
    }

    public float ProcessStat(Stat stat, float value)
    {
        var processedValue = value;

        foreach (EffectWithDuration effectWithDuration in _activeEffects)
        {
            processedValue = effectWithDuration.Effect.ProcessStat(stat, processedValue);
        }

        foreach (Effect passiveEffect in _passiveEffects.Values)
        {
            processedValue = passiveEffect.ProcessStat(stat, processedValue);
        }

        return processedValue;
    }

    public void AddActiveEffect(Effect effect, float duration)
    {
        _activeEffects.Add(new EffectWithDuration(effect, duration));
        _statsManager.ClearCache();
    }

    public Guid AddPassiveEffect(Effect effect)
    {
        Guid effectId = Guid.NewGuid();
        _passiveEffects.Add(effectId, effect);
        _statsManager.ClearCache();

        return effectId;
    }

    public void RemovePassiveEffect(Guid effectId)
    {
        _passiveEffects[effectId].Finish(_actor);
        _passiveEffects.Remove(effectId);
        _statsManager.ClearCache();
    }
}
