using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EffectManager
{
    private readonly Actor actor;
    
    private readonly EnumDictionary<ActiveEffect, bool> activeEffectStatusLookup = new EnumDictionary<ActiveEffect, bool>(false);
    private readonly Dictionary<ActiveEffect, float> totalActiveEffectDurations = new Dictionary<ActiveEffect, float>();
    private readonly Dictionary<ActiveEffect, float> remainingActiveEffectDurations = new Dictionary<ActiveEffect, float>();

    private readonly Dictionary<Guid, PassiveEffect> passiveEffects = new Dictionary<Guid, PassiveEffect>();

    private readonly EnumDictionary<StackingEffect, int> stacks = new EnumDictionary<StackingEffect, int>(0);
    private readonly Dictionary<StackingEffect, float> remainingStackingEffectDurations = new Dictionary<StackingEffect, float>();

    public Subject<ActiveEffect> ActiveEffectAddedSubject { get; } = new Subject<ActiveEffect>();
    public Subject<ActiveEffect> ActiveEffectRemovedSubject { get; } = new Subject<ActiveEffect>();

    public Subject<PassiveEffectData> PassiveEffectAddedSubject { get; } = new Subject<PassiveEffectData>();
    public Subject<PassiveEffectData> PassiveEffectRemovedSubject { get; } = new Subject<PassiveEffectData>();

    public Subject<StackingEffect> StackingEffectAddedSubject { get; } = new Subject<StackingEffect>();
    public Subject<StackingEffect> StackingEffectRemovedSubject { get; } = new Subject<StackingEffect>();

    public EffectManager(Actor actor, Subject updateSubject)
    {
        this.actor = actor;

        ActiveEffectAddedSubject.Subscribe(_ => actor.StatsManager.ClearCache());
        ActiveEffectRemovedSubject.Subscribe(_ => actor.StatsManager.ClearCache());
        PassiveEffectAddedSubject.Subscribe(_ => actor.StatsManager.ClearCache());
        PassiveEffectRemovedSubject.Subscribe(_ => actor.StatsManager.ClearCache());
        StackingEffectAddedSubject.Subscribe(_ => actor.StatsManager.ClearCache());
        StackingEffectRemovedSubject.Subscribe(_ => actor.StatsManager.ClearCache());

        actor.StatsManager.RegisterPipe(new ActiveSlowHandler(actor));
        actor.StatsManager.RegisterPipe(new PassiveSlowHandler(actor));
        actor.StatsManager.RegisterPipe(new VulnerableHandler(actor));

        BleedHandler bleedHandler = new BleedHandler(actor, this);
        PoisonHandler poisonHandler = new PoisonHandler(actor, this);
        
        updateSubject.Subscribe(() =>
        {
            bleedHandler.Update();
            poisonHandler.Update();
            
            TickActiveEffects();
            TickStackingEffects();
        });

        actor.DeathSubject.Subscribe(() =>
        {
            RemoveAllActiveEffects();
            RemoveAllPassiveEffects();
            RemoveAllStackingEffects();
        });
    }

    public void AddActiveEffect(ActiveEffect effect, float duration)
    {
        if (actor.Dead) return;

        if (!activeEffectStatusLookup[effect])
        {
            activeEffectStatusLookup[effect] = true;
            totalActiveEffectDurations[effect] = duration;
            remainingActiveEffectDurations[effect] = duration;
            ActiveEffectAddedSubject.Next(effect);
            return;
        }

        if (duration > remainingActiveEffectDurations[effect])
        {
            totalActiveEffectDurations[effect] = duration;
            remainingActiveEffectDurations[effect] = duration;
            ActiveEffectAddedSubject.Next(effect);
            return;
        }

        ActiveEffectAddedSubject.Next(effect);
    }

    public void RemoveActiveEffect(ActiveEffect effect)
    {
        if (!activeEffectStatusLookup[effect]) return;
        
        activeEffectStatusLookup[effect] = false;
        totalActiveEffectDurations.Remove(effect);
        remainingActiveEffectDurations.Remove(effect);
        ActiveEffectRemovedSubject.Next(effect);
    }

    public void RemoveAllActiveEffects() => EnumUtils.ForEach<ActiveEffect>(RemoveActiveEffect);

    public bool HasActiveEffect(ActiveEffect effect) => activeEffectStatusLookup[effect];

    public float GetTotalActiveEffectDuration(ActiveEffect effect) => totalActiveEffectDurations[effect];

    public float GetRemainingActiveEffectDuration(ActiveEffect effect) => remainingActiveEffectDurations[effect];

    public bool TryAddPassiveEffect(PassiveEffect effect, out Guid id)
    {
        if (actor.Dead)
        {
            id = Guid.Empty;
            return false;
        }

        id = Guid.NewGuid();
        passiveEffects[id] = effect;
        PassiveEffectAddedSubject.Next(new PassiveEffectData(id, effect));
        return true;
    }

    public void RemovePassiveEffect(Guid id)
    {
        PassiveEffect effect = passiveEffects[id];
        passiveEffects.Remove(id);
        PassiveEffectRemovedSubject.Next(new PassiveEffectData(id, effect));
    }

    public void RemoveAllPassiveEffects() => passiveEffects.Keys.ToList().ForEach(RemovePassiveEffect);

    public bool HasPassiveEffect(PassiveEffect effect)
    {
        foreach (PassiveEffect existingEffect in passiveEffects.Values)
        {
            if (existingEffect == effect) return true;
        }

        return false;
    }

    public PassiveEffect GetPassiveEffect(Guid id) => passiveEffects[id];

    public void AddStack(StackingEffect effect) => AddStacks(effect, 1);

    public void AddStacks(StackingEffect effect, int stackCount)
    {
        if (actor.Dead || stackCount <= 0) return;

        stacks[effect] = EffectLookup.Instance.HasMaxStackSize(effect)
            ? Math.Min(EffectLookup.Instance.GetMaxStackSize(effect), stacks[effect] + stackCount)
            : stacks[effect] + stackCount;

        remainingStackingEffectDurations[effect] = EffectLookup.Instance.GetStackingEffectDuration(effect);
        StackingEffectAddedSubject.Next(effect);
    }

    public void RemoveStackingEffect(StackingEffect effect)
    {
        if (stacks[effect] == 0) return;
        
        stacks[effect] = 0;
        remainingStackingEffectDurations.Remove(effect);
        StackingEffectRemovedSubject.Next(effect);
    }

    public void RemoveAllStackingEffects() => EnumUtils.ForEach<StackingEffect>(RemoveStackingEffect);

    public int GetStacks(StackingEffect effect) => stacks[effect];

    public float GetRemainingStackingEffectDuration(StackingEffect effect) => remainingStackingEffectDurations[effect];

    private void TickActiveEffects()
    {
        EnumUtils.ForEach((ActiveEffect effect) =>
        {
            if (!activeEffectStatusLookup[effect]) return;

            remainingActiveEffectDurations[effect] -= Time.deltaTime;

            if (remainingActiveEffectDurations[effect] <= 0) RemoveActiveEffect(effect);
        });
    }

    private void TickStackingEffects()
    {
        EnumUtils.ForEach((StackingEffect effect) =>
        {
            if (stacks[effect] == 0) return;

            remainingStackingEffectDurations[effect] -= Time.deltaTime;

            if (remainingStackingEffectDurations[effect] <= 0) RemoveStackingEffect(effect);
        });
    }
}
