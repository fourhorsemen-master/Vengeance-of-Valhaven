using System;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : StatPipe
{
    private List<EffectWithDuration> _activeEffects;
    private Dictionary<Guid, Effect> _passiveEffects;
    private readonly Actor _actor;
    private readonly StatsManager _statsManager;

    public EffectManager(Actor actor, Subject updateSubject, StatsManager statsManager)
    {
        _activeEffects = new List<EffectWithDuration>();
        _passiveEffects = new Dictionary<Guid, Effect>();
        _actor = actor;
        _statsManager = statsManager;
        _statsManager.RegisterPipe(this);
        updateSubject.Subscribe(() =>
        {
            ForEachEffect(e => e.Update(_actor));
            TickActiveEffects();
        });
    }

    /// <summary>
    /// Adds an active effect to the actor, this effect will last for the given duration.
    /// </summary>
    /// <param name="effect"> The effect to add. </param>
    /// <param name="duration"> The duration of the effect. </param>
    public void AddActiveEffect(Effect effect, float duration)
    {
        effect.Start(_actor);
        _activeEffects.Add(new EffectWithDuration(effect, duration));
        _statsManager.ClearCache();
    }

    /// <summary>
    /// Adds a passive effect to the actor. The passive effect can be removed using the returned Guid.
    /// </summary>
    /// <param name="effect"> The effect to add. </param>
    /// <returns> The Guid to use to remove the effect. </returns>
    public Guid AddPassiveEffect(Effect effect)
    {
        effect.Start(_actor);

        Guid effectId = Guid.NewGuid();
        _passiveEffects.Add(effectId, effect);
        _statsManager.ClearCache();

        return effectId;
    }

    /// <summary>
    /// This will remove the effect with the given Guid from the actor. This id is the one returned from
    /// the AddPassiveEffect method.
    /// </summary>
    /// <param name="effectId"> The id of the effect to remove. </param>
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
        ForEachEffect(e => value = e.ProcessStat(stat, value));
        return value;
    }

    public int ProcessOutgoingDamage(int damage)
    {
        ForEachEffect(e => damage = e.ProcessOutgoingDamage(damage));
        return damage;
    }

    public int ProcessIncomingDamage(int damage)
    {
        ForEachEffect(e => damage = e.ProcessIncomingDamage(damage));
        return damage;
    }

    public int ProcessIncomingHeal(int healing)
    {
        ForEachEffect(e => healing = e.ProcessIncomingHeal(healing));
        return healing;
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

    private void ForEachEffect(Action<Effect> action)
    {
        foreach (EffectWithDuration effectWithDuration in _activeEffects)
        {
            action(effectWithDuration.Effect);
        }

        foreach (Effect passiveEffect in _passiveEffects.Values)
        {
            action(passiveEffect);
        }
    }
}
