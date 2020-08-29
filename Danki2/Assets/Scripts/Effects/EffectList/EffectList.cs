using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class EffectList : MonoBehaviour
{
    protected abstract Actor Actor { get; }

    protected abstract EffectListItem EffectListItemPrefab { get; }

    private readonly Dictionary<Guid, EffectListItem> effectListItems = new Dictionary<Guid, EffectListItem>();

    private void Start()
    {
        Actor.EffectManager.EffectAddedSubject.Subscribe(AddEffectListItem);
        Actor.EffectManager.EffectRemovedSubject.Subscribe(RemoveEffectListItem);
    }

    private void AddEffectListItem(Guid id)
    {
        if (!Actor.EffectManager.TryGetEffect(id, out Effect effect)) return;
        
        EffectListItem effectListItem = Instantiate(EffectListItemPrefab, transform).Initialise(effect);
        effectListItems.Add(id, effectListItem);
    }

    private void RemoveEffectListItem(Guid id)
    {
        if (!effectListItems.TryGetValue(id, out EffectListItem effectListItem)) return;
        
        effectListItem.Destroy();
        effectListItems.Remove(id);
    }
}
