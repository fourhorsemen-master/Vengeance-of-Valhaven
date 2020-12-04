using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Keeps track of entities of given type, which can be added indefinitely or temporarily (in which case they will be ticked down and expired).
/// It exposes methods for adding, and for getting and removing the entities either by id or by predicate.
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public class Registry<TEntity>
{
    private readonly Action<Guid, TEntity> onEntityAdded = (id, effect) => { };
    private readonly Action<Guid, TEntity> onEntityRemoved = (id, effect) => { };

    private readonly Dictionary<Guid, TEntity> entities = new Dictionary<Guid, TEntity>();

    private readonly Dictionary<Guid, float> totalDurations = new Dictionary<Guid, float>();

    private readonly Dictionary<Guid, float> durations = new Dictionary<Guid, float>();

    public Registry(Subject updateSubject, Action<Guid, TEntity> onEffectAdded = null, Action<Guid, TEntity> onEffectRemoved = null)
    {
        updateSubject.Subscribe(TickDurations);

        if (onEffectAdded != null)
        {
            this.onEntityAdded = onEffectAdded;
        }

        if (onEffectRemoved != null)
        {
            this.onEntityRemoved = onEffectRemoved;
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
            entities.Remove(id);
            durations.Remove(id);
            totalDurations.Remove(id);

            onEntityRemoved(id, entity);
        }
    }

    public void RemoveWhere(Predicate<TEntity> predicate)
    {
        entities.Keys.ToList()
            .Where(id => predicate(entities[id]))
            .ForEach(Remove);
    }

    public void Clear() => RemoveWhere(_ => true);

    private Guid Add(TEntity entity, float? duration = null)
    {
        Guid id = Guid.NewGuid();

        entities.Add(id, entity);

        if (duration.HasValue)
        {
            durations.Add(id, duration.Value);
            totalDurations.Add(id, duration.Value);
        }

        onEntityAdded(id, entity);

        return id;
    }

    private void TickDurations()
    {
        durations.Keys.ToList()
            .Where(id =>
            {
                durations[id] -= Time.deltaTime;
                return durations[id] <= 0;
            })
            .ForEach(Remove);
    }
}
