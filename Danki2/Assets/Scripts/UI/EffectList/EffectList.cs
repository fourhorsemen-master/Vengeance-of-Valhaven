using System;
using System.Collections.Generic;
using System.Linq;
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

    private void Update()
    {
        foreach (KeyValuePair<Guid,EffectListItem> keyValuePair in effectListItems)
        {
            Guid id = keyValuePair.Key;
            EffectListItem effectListItem = keyValuePair.Value;

            if (!Actor.EffectManager.TryGetRemainingDuration(id, out float remainingDuration)) return;

            effectListItem.SetRemainingDuration(remainingDuration);
        }
    }

    private void AddEffectListItem(Guid id)
    {
        if (!Actor.EffectManager.TryGetEffect(id, out Effect effect))
        {
            Debug.LogError($"Tried to add effect list item with id: {id.ToString()}, when this does not exist in the effect manager.");
            return;
        }

        EffectListItem effectListItem = Instantiate(EffectListItemPrefab, transform);
        effectListItems.Add(id, effectListItem);

        if (Actor.EffectManager.TryGetTotalDuration(id, out float totalDuration))
        {
            effectListItem.Initialise(effect, totalDuration);
            return;
        }

        effectListItem.Initialise(effect);
    }

    private void RemoveAllEffectListItems()
    {
        effectListItems.Keys.ToList().ForEach(RemoveEffectListItem);
    }

    private void RemoveEffectListItem(Guid id)
    {
        if (!effectListItems.TryGetValue(id, out EffectListItem effectListItem))
        {
            Debug.LogError($"Tried to remove effect list item with id: {id.ToString()}, when this does not exist in the effect manager.");
            return;
        }
        
        effectListItem.Destroy();
        effectListItems.Remove(id);
    }
}
