using System;
using UnityEngine;

public class EffectManager : IStatPipe, IMovementStatusProvider
{
    private readonly Actor actor;
    private readonly StatsManager statsManager;

    private readonly Registry<Effect> effects;

    public Subject<Guid> EffectAddedSubject { get; } = new Subject<Guid>();
    public Subject<Guid> EffectRemovedSubject { get; }  = new Subject<Guid>();

    public EffectManager(Actor actor, Subject updateSubject, StatsManager statsManager)
    {
        this.actor = actor;
        this.statsManager = statsManager;
        this.statsManager.RegisterPipe(this);

        effects = new Registry<Effect>(
            updateSubject,
            (id, e) => {
                e.Start(actor);
                EffectAddedSubject.Next(id);
                statsManager.ClearCache();
            },
            (id, e) => {
                e.Finish(actor);
                EffectRemovedSubject.Next(id);
                statsManager.ClearCache();
            }
        );

        updateSubject.Subscribe(() =>
        {
            effects.ForEach(e => e.Update(this.actor));
        });

        actor.DeathSubject.Subscribe(effects.Clear);
    }

    public bool TryGetEffect(Guid id, out Effect effect) => effects.TryGet(id, out effect);

    public bool TryGetTotalDuration(Guid id, out float totalDuration) => effects.TryGetTotalDuration(id, out totalDuration);

    public bool TryGetRemainingDuration(Guid id, out float remainingDuration) => effects.TryGetRemainingDuration(id, out remainingDuration);

    public void AddActiveEffect(Effect effect, float duration)
    {
        if (actor.Dead) return;

        effects.AddTemporary(effect, duration);
    }

    public bool TryAddPassiveEffect(Effect effect, out Guid id)
    {
        if (actor.Dead)
        {
            id = default;
            return false;
        }

        id = effects.AddIndefinite(effect);

        return true;
    }

    public void RemoveEffect(Guid id) => effects.Remove(id);

    public float ProcessStat(Stat stat, float value)
    {
        effects.ForEach(e => value += e.GetLinearStatModifier(stat));
        effects.ForEach(e => value *= e.GetMultiplicativeStatModifier(stat));
        return value;
    }

    public int ProcessOutgoingDamage(int damage)
    {
        float floatDamage = damage;
        effects.ForEach(e => floatDamage += e.GetLinearOutgoingDamageModifier());
        effects.ForEach(e => floatDamage *= e.GetMultiplicativeOutgoingDamageModifier());
        return Mathf.RoundToInt(floatDamage);
    }

    public int ProcessIncomingDamage(int damage)
    {
        float floatDamage = damage;
        effects.ForEach(e => floatDamage += e.GetLinearIncomingDamageModifier());
        effects.ForEach(e => floatDamage *= e.GetMultiplicativeIncomingDamageModifier());
        return Mathf.RoundToInt(floatDamage);
    }

    public int ProcessIncomingHeal(int healing)
    {
        float floatHealing = healing;
        effects.ForEach(e => floatHealing += e.GetLinearIncomingHealModifier());
        effects.ForEach(e => floatHealing *= e.GetMultiplicativeIncomingHealModifier());
        return Mathf.RoundToInt(floatHealing);
    }

    public bool Stuns()
    {
        bool setStunned = false;

        effects.ForEach(e => {
            if (e.Stuns) setStunned = true;
        });

        return setStunned;
    }

    public bool Roots()
    {
        bool setRooted = false;

        effects.ForEach(e => {
            if (e.Roots) setRooted = true;
        });

        return setRooted;
    }
}
