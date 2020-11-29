using System;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager
{
    private readonly EnumDictionary<ActiveEffect, bool> activeEffectStatusLookup = new EnumDictionary<ActiveEffect, bool>(false);
    private readonly Dictionary<ActiveEffect, float> totalActiveEffectDurations = new Dictionary<ActiveEffect, float>();
    private readonly Dictionary<ActiveEffect, float> remainingActiveEffectDurations = new Dictionary<ActiveEffect, float>();

    private readonly Dictionary<Guid, PassiveEffect> passiveEffects = new Dictionary<Guid, PassiveEffect>();

    private readonly EnumDictionary<StackingEffect, int> stacks = new EnumDictionary<StackingEffect, int>(0);
    private readonly Dictionary<StackingEffect, float> remainingStackingEffectDurations = new Dictionary<StackingEffect, float>();

    public EffectManager(Subject updateSubject)
    {
        updateSubject.Subscribe(() =>
        {
            TickActiveEffects();
            TickStackingEffects();
        });
    }

    public void AddActiveEffect(ActiveEffect effect, float duration)
    {
        if (activeEffectStatusLookup[effect] && remainingActiveEffectDurations[effect] < duration)
        {
            totalActiveEffectDurations[effect] = duration;
            remainingActiveEffectDurations[effect] = duration;
            return;
        }

        activeEffectStatusLookup[effect] = true;
        totalActiveEffectDurations[effect] = duration;
        remainingActiveEffectDurations[effect] = duration;
    }

    public void RemoveActiveEffect(ActiveEffect effect)
    {
        activeEffectStatusLookup[effect] = false;
        totalActiveEffectDurations.Remove(effect);
        remainingActiveEffectDurations.Remove(effect);
    }

    public bool HasActiveEffect(ActiveEffect effect) => activeEffectStatusLookup[effect];

    public bool TryGetTotalActiveEffectDuration(ActiveEffect effect, out float totalDuration) =>
        totalActiveEffectDurations.TryGetValue(effect, out totalDuration);

    public bool TryGetRemainingActiveEffectDuration(ActiveEffect effect, out float remainingDuration) =>
        remainingActiveEffectDurations.TryGetValue(effect, out remainingDuration);

    public Guid AddPassiveEffect(PassiveEffect effect)
    {
        Guid id = Guid.NewGuid();
        passiveEffects[id] = effect;
        return id;
    }

    public void RemovePassiveEffect(Guid id)
    {
        passiveEffects.Remove(id);
    }

    public bool HasPassiveEffect(PassiveEffect effect)
    {
        foreach (PassiveEffect existingEffect in passiveEffects.Values)
        {
            if (existingEffect == effect) return true;
        }

        return false;
    }

    public void AddStack(StackingEffect effect)
    {
        bool hasMaxStackSize = EffectLookup.Instance.HasMaxStackSize(effect);
        int maxStackSize = EffectLookup.Instance.GetMaxStackSize(effect);
        if (hasMaxStackSize && stacks[effect] < maxStackSize) stacks[effect]++;

        remainingStackingEffectDurations[effect] = EffectLookup.Instance.GetStackingEffectDuration(effect);
    }

    public void RemoveStackingEffect(StackingEffect effect)
    {
        stacks[effect] = 0;
        remainingStackingEffectDurations.Remove(effect);
    }

    public int GetStacks(StackingEffect effect) => stacks[effect];

    public bool TryGetRemainingStackingEffectDuration(StackingEffect effect, out float remainingDuration) =>
        remainingStackingEffectDurations.TryGetValue(effect, out remainingDuration);

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
