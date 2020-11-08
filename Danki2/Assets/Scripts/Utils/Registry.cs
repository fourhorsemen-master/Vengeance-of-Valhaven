using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Registry<TEntity>
{
    private readonly Action<Guid, TEntity> onEffectAdded = (id, effect) => { };
    private readonly Action<Guid, TEntity> onEffectRemoved = (id, effect) => { };

    private Dictionary<Guid, TEntity> entities = new Dictionary<Guid, TEntity>();

    private Dictionary<Guid, float> totalDurations = new Dictionary<Guid, float>();

    private Dictionary<Guid, float> durations = new Dictionary<Guid, float>();

    public Registry(Subject updateSubject, Action<Guid, TEntity> onEffectAdded = null, Action<Guid, TEntity> onEffectRemoved = null)
    {
        updateSubject.Subscribe(TickDurations);

        if (onEffectAdded != null)
        {
            this.onEffectAdded = onEffectAdded;
        }

        if (onEffectRemoved != null)
        {
            this.onEffectRemoved = onEffectRemoved;
        }
    }

    public bool TryGet(Guid id, out TEntity entity) => entities.TryGetValue(id, out entity);

    public bool TryGetTotalDuration(Guid id, out float totalDuration) => totalDurations.TryGetValue(id, out totalDuration);

    public bool TryGetRemainingDuration(Guid id, out float remainingDuration) => durations.TryGetValue(id, out remainingDuration);

    public void ForEach(Action<TEntity> action, Predicate<TEntity> filter = null)
    {
        foreach (TEntity entity in entities.Values)
        {
            if (filter == null || filter(entity))
            {
                action(entity);
            }
        }
    }

    public Guid AddIndefinite(TEntity entity) => Add(entity);

    public Guid AddTemporary(TEntity entity, float duration) => Add(entity, duration);

    public void Remove(Guid id)
    {
        if (entities.TryGetValue(id, out TEntity entity))
        {
            onEffectRemoved(id, entity);

            entities.Remove(id);
            durations.Remove(id);
            totalDurations.Remove(id);
        }
    }

    public void RemoveWhere(Predicate<TEntity> predicate)
    {
        foreach (Guid id in entities.Keys)
        {
            if (predicate(entities[id]))
            {
                Remove(id);
            }
        }
    }

    public void Clear()
    {
        entities.Clear();
        totalDurations.Clear();
        durations.Clear();
    }

    private Guid Add(TEntity entity, float? duration = null)
    {
        Guid id = Guid.NewGuid();

        entities.Add(id, entity);

        if (duration.HasValue)
        {
            durations.Add(id, duration.Value);
            totalDurations.Add(id, duration.Value);
        }

        onEffectAdded(id, entity);

        return id;
    }

    private void TickDurations()
    {
        durations.Keys.ToList().ForEach(id =>
        {
            durations[id] -= Time.deltaTime;

            if (durations[id] <= 0)
            {
                Remove(id);
            }
        });
    }
}