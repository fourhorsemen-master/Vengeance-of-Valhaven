using System;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager2
{
    private readonly EnumDictionary<Effect2, bool> activeEffectStatusLookup = new EnumDictionary<Effect2, bool>(false);
    private readonly Dictionary<Effect2, float> totalActiveEffectDurations = new Dictionary<Effect2, float>();
    private readonly Dictionary<Effect2, float> remainingActiveEffectDurations = new Dictionary<Effect2, float>();

    private readonly Dictionary<Guid, Effect2> passiveEffects = new Dictionary<Guid, Effect2>();

    private readonly EnumDictionary<StackingEffect, int> stacks = new EnumDictionary<StackingEffect, int>(0);
    private readonly Dictionary<StackingEffect, float> remainingStackingEffectDurations = new Dictionary<StackingEffect, float>();

    public EffectManager2(Subject updateSubject)
    {
        updateSubject.Subscribe(() =>
        {
            TickActiveEffects();
            TickStackingEffects();
        });
    }

    public void AddActiveEffect(Effect2 effect, float duration)
    {
        if (activeEffectStatusLookup[effect])
        {
            totalActiveEffectDurations[effect] = Mathf.Max(totalActiveEffectDurations[effect], duration);
            remainingActiveEffectDurations[effect] = Mathf.Max(remainingActiveEffectDurations[effect], duration);
            return;
        }

        activeEffectStatusLookup[effect] = true;
        totalActiveEffectDurations[effect] = duration;
        remainingActiveEffectDurations[effect] = duration;
    }

    public void RemoveActiveEffect(Effect2 effect)
    {
        activeEffectStatusLookup[effect] = false;
        totalActiveEffectDurations.Remove(effect);
        remainingActiveEffectDurations.Remove(effect);
    }

    public bool HasActiveEffect(Effect2 effect) => activeEffectStatusLookup[effect];

    public bool TryGetTotalActiveEffectDuration(Effect2 effect, out float totalDuration) =>
        totalActiveEffectDurations.TryGetValue(effect, out totalDuration);

    public bool TryGetRemainingActiveEffectDuration(Effect2 effect, out float remainingDuration) =>
        remainingActiveEffectDurations.TryGetValue(effect, out remainingDuration);

    public Guid AddPassiveEffect(Effect2 effect)
    {
        Guid id = Guid.NewGuid();
        passiveEffects[id] = effect;
        return id;
    }

    public void RemovePassiveEffect(Guid id)
    {
        passiveEffects.Remove(id);
    }

    public bool HasPassiveEffect(Effect2 effect)
    {
        foreach (Effect2 passiveEffect in passiveEffects.Values)
        {
            if (passiveEffect == effect) return true;
        }

        return false;
    }

    public void AddStack(StackingEffect effect)
    {
        if (stacks[effect] < GetMaximumStackSize(effect)) stacks[effect]++;
        remainingStackingEffectDurations[effect] = GetTotalStackingDuration(effect);
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
        EnumUtils.ForEach((Effect2 effect) =>
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

    private float GetTotalStackingDuration(StackingEffect effect)
    {
        return 5;
    }

    private int GetMaximumStackSize(StackingEffect effect)
    {
        return 3;
    }
}
