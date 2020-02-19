using System;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : StatPipe
{
    private List<EffectWithDuration> _activeEffects;
    private Dictionary<Guid, Effect> _passiveEffects;
    private readonly Actor _actor;
    private readonly StatsManager _statsManager;

    public EffectManager(Actor actor, StatsManager statsManager)
    {
        _activeEffects = new List<EffectWithDuration>();
        _passiveEffects = new Dictionary<Guid, Effect>();
        _actor = actor;
        _statsManager = statsManager;
        _statsManager.RegisterPipe(this);
    }
        
    public void ProcessEffects()
    {
        ForEachEffect(e => e.Update(_actor));
        TickActiveEffects();
    }

    private void TickActiveEffects()
    {
        bool someExpired = false;

        _activeEffects = _activeEffects.FindAll(activeEffect =>
        {
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
        if (!_passiveEffects.ContainsKey(effectId))
        {
            Debug.LogError($"Tried to remove passive effect with id that could not be found. Id: {effectId.ToString()}");
            return;
        }

        _passiveEffects[effectId].Finish(_actor);
        _passiveEffects.Remove(effectId);
        _statsManager.ClearCache();
    }

    public float ProcessStat(Stat stat, float value)
    {
        var processedValue = value;

        ForEachEffect(e => processedValue = e.ProcessStat(stat, processedValue));

        return processedValue;
    }

    private void ForEachEffect(Action<Effect> action)
    {
        foreach (EffectWithDuration effectWithDuration in _activeEffects)
        {
            action.Invoke(effectWithDuration.Effect);
        }

        foreach (Effect passiveEffect in _passiveEffects.Values)
        {
            action.Invoke(passiveEffect);
        }
    }
}
