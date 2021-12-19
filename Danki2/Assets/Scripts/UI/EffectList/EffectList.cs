using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class EffectList : MonoBehaviour
{
    protected abstract Actor Actor { get; }

    protected abstract EffectListItem EffectListItemPrefab { get; }

    private readonly Dictionary<ActiveEffect, EffectListItem> activeEffects = new Dictionary<ActiveEffect, EffectListItem>();
    private readonly Dictionary<Guid, EffectListItem> passiveEffects = new Dictionary<Guid, EffectListItem>();
    private readonly Dictionary<StackingEffect, EffectListItem> stackingEffects = new Dictionary<StackingEffect, EffectListItem>();

    private void Start()
    {
        Actor.EffectManager.ActiveEffectAddedSubject.Subscribe(ActiveEffectAdded);
        Actor.EffectManager.ActiveEffectRemovedSubject.Subscribe(ActiveEffectRemoved);
        
        Actor.EffectManager.PassiveEffectAddedSubject.Subscribe(PassiveEffectAdded);
        Actor.EffectManager.PassiveEffectRemovedSubject.Subscribe(PassiveEffectRemoved);
        
        Actor.EffectManager.StackingEffectAddedSubject.Subscribe(StackingEffectAdded);
        Actor.EffectManager.StackingEffectRemovedSubject.Subscribe(StackingEffectRemoved);
    }

    private void Update()
    {
        foreach (ActiveEffect effect in activeEffects.Keys)
        {
            activeEffects[effect].UpdateRemainingDuration(Actor.EffectManager.GetRemainingActiveEffectDuration(effect));
        }

        foreach (StackingEffect effect in stackingEffects.Keys)
        {
            stackingEffects[effect].UpdateRemainingDuration(Actor.EffectManager.GetRemainingStackingEffectDuration(effect));
        }
    }

    private void ActiveEffectAdded(ActiveEffect effect)
    {
        if (activeEffects.TryGetValue(effect, out EffectListItem existingEffectListItem))
        {
            existingEffectListItem.ResetTotalDuration(Actor.EffectManager.GetTotalActiveEffectDuration(effect));
            return;
        }

        activeEffects[effect] = Instantiate(EffectListItemPrefab, transform)
            .InitialiseActiveEffect(effect, Actor.EffectManager.GetTotalActiveEffectDuration(effect));
    }

    private void ActiveEffectRemoved(ActiveEffect effect)
    {
        activeEffects[effect].Destroy();
        activeEffects.Remove(effect);
    }

    private void PassiveEffectAdded(PassiveEffectData effectData)
    {
        passiveEffects[effectData.Id] = Instantiate(EffectListItemPrefab, transform)
            .InitialisePassiveEffect(effectData.Effect);
    }

    private void PassiveEffectRemoved(PassiveEffectData effectData)
    {
        passiveEffects[effectData.Id].Destroy();
        passiveEffects.Remove(effectData.Id);
    }

    private void StackingEffectAdded(StackingEffect effect)
    {
        if (stackingEffects.TryGetValue(effect, out EffectListItem existingEffectListItem))
        {
            existingEffectListItem.UpdateStacks(Actor.EffectManager.GetStacks(effect));
        }
        else if (Actor.EffectManager.GetStacks(effect) > 0)
        {
            stackingEffects[effect] = Instantiate(EffectListItemPrefab, transform)
               .InitialiseStackingEffect(effect, Actor.EffectManager.GetStacks(effect));
        }
    }

    private void StackingEffectRemoved(StackingEffect effect)
    {
        stackingEffects[effect].Destroy();
        stackingEffects.Remove(effect);
    }
}
