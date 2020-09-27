using System;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : IStatPipe, IMovementStatusProvider
{
    private readonly Actor actor;
    private readonly StatsManager statsManager;

    private readonly Dictionary<Guid, Effect> effects = new Dictionary<Guid, Effect>();
    private readonly Dictionary<Guid, float> totalDurations = new Dictionary<Guid, float>();
    private readonly Dictionary<Guid, float> remainingDurations = new Dictionary<Guid, float>();

    public Subject<Guid> EffectAddedSubject { get; } = new Subject<Guid>();
    public Subject<Guid> EffectRemovedSubject { get; }  = new Subject<Guid>();

    public EffectManager(Actor actor, Subject updateSubject, StatsManager statsManager)
    {
        this.actor = actor;
        this.statsManager = statsManager;
        this.statsManager.RegisterPipe(this);
        updateSubject.Subscribe(() =>
        {
            ForEachEffect(e => e.Update(this.actor));
            TickActiveEffects();
        });
    }

    public bool TryGetEffect(Guid id, out Effect effect) => effects.TryGetValue(id, out effect);

    public bool TryGetTotalDuration(Guid id, out float totalDuration) => totalDurations.TryGetValue(id, out totalDuration);

    public bool TryGetRemainingDuration(Guid id, out float remainingDuration) => remainingDurations.TryGetValue(id, out remainingDuration);

    /// <summary>
    /// Adds an active effect to the actor, this effect will last for the given duration.
    /// </summary>
    /// <param name="effect"> The effect to add. </param>
    /// <param name="duration"> The duration of the effect. </param>
    public void AddActiveEffect(Effect effect, float duration)
    {
        effect.Start(actor);

        Guid id = Guid.NewGuid();
        effects.Add(id, effect);
        totalDurations.Add(id, duration);
        remainingDurations.Add(id, duration);
        EffectAddedSubject.Next(id);

        statsManager.ClearCache();
    }

    /// <summary>
    /// Adds a passive effect to the actor. The passive effect can be removed using the returned Guid.
    /// </summary>
    /// <param name="effect"> The effect to add. </param>
    /// <returns> The Guid to use to remove the effect. </returns>
    public Guid AddPassiveEffect(Effect effect)
    {
        effect.Start(actor);

        Guid id = Guid.NewGuid();
        effects.Add(id, effect);
        EffectAddedSubject.Next(id);

        statsManager.ClearCache();

        return id;
    }

    /// <summary>
    /// This will remove the effect with the given Guid from the actor. This id is the one returned from
    /// the AddPassiveEffect method.
    /// </summary>
    /// <param name="id"> The id of the effect to remove. </param>
    public void RemovePassiveEffect(Guid id)
    {
        if (!effects.ContainsKey(id))
        {
            Debug.LogError($"Tried to remove passive effect with id that could not be found. Id: {id.ToString()}");
            return;
        }

        if (remainingDurations.ContainsKey(id))
        {
            Debug.LogError($"Tried to remove active effect with id. Id: {id.ToString()}");
            return;
        }

        effects[id].Finish(actor);
        effects.Remove(id);
        EffectRemovedSubject.Next(id);

        statsManager.ClearCache();
    }

    public float ProcessStat(Stat stat, float value)
    {
        ForEachEffect(e => value += e.GetLinearStatModifier(stat));
        ForEachEffect(e => value *= e.GetMultiplicativeStatModifier(stat));
        return value;
    }

    public int ProcessOutgoingDamage(int damage)
    {
        float floatDamage = damage;
        ForEachEffect(e => floatDamage += e.GetLinearOutgoingDamageModifier());
        ForEachEffect(e => floatDamage *= e.GetMultiplicativeOutgoingDamageModifier());
        return Mathf.RoundToInt(floatDamage);
    }

    public int ProcessIncomingDamage(int damage)
    {
        float floatDamage = damage;
        ForEachEffect(e => floatDamage += e.GetLinearIncomingDamageModifier());
        ForEachEffect(e => floatDamage *= e.GetMultiplicativeIncomingDamageModifier());
        return Mathf.RoundToInt(floatDamage);
    }

    public int ProcessIncomingHeal(int healing)
    {
        float floatHealing = healing;
        ForEachEffect(e => floatHealing += e.GetLinearIncomingHealModifier());
        ForEachEffect(e => floatHealing *= e.GetMultiplicativeIncomingHealModifier());
        return Mathf.RoundToInt(floatHealing);
    }

    // IMovementStatusProvider method:
    public bool Stuns()
    {
        bool setStunned = false;

        ForEachEffect(e => {
            if (e.Stuns) setStunned = true;
        });

        return setStunned;
    }

    // IMovementStatusProvider method:
    public bool Roots()
    {
        bool setRooted = false;

        ForEachEffect(e => {
            if (e.Roots) setRooted = true;
        });

        return setRooted;
    }

    private void TickActiveEffects()
    {
        List<Guid> expiredEffectIds = new List<Guid>();

        ForEachEffectId(id =>
        {
            if (!TryGetRemainingDuration(id, out float remainingDuration)) return;

            remainingDurations[id] = remainingDuration - Time.deltaTime;
            if (remainingDurations[id] <= 0)
            {
                expiredEffectIds.Add(id);
            }
        });

        if (expiredEffectIds.Count == 0) return;
        
        statsManager.ClearCache();
        
        expiredEffectIds.ForEach(id =>
        {
            effects[id].Finish(actor);
            effects.Remove(id);
            totalDurations.Remove(id);
            remainingDurations.Remove(id);
            EffectRemovedSubject.Next(id);
        });
    }

    private void ForEachEffect(Action<Effect> action)
    {
        foreach (Effect effect in effects.Values)
        {
            action(effect);
        }
    }

    private void ForEachEffectId(Action<Guid> action)
    {
        foreach (Guid id in effects.Keys)
        {
            action(id);
        }
    }
}
