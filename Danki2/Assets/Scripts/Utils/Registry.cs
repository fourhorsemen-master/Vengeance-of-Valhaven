using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Registry<TEntity>
{
    private readonly Action<Guid, TEntity> onEffectAdded = (id, effect) => { };
    private readonly Action<Guid, TEntity> onEffectRemoved = (id, effect) => { };

    public Dictionary<Guid, TEntity> Entities { get; } = new Dictionary<Guid, TEntity>();

    public Dictionary<Guid, float> TotalDurations { get; } = new Dictionary<Guid, float>();

    public Dictionary<Guid, float> Durations { get; } = new Dictionary<Guid, float>();

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

    public bool TryGet(Guid id, out TEntity entity) => Entities.TryGetValue(id, out entity);

    public bool TryGetTotalDuration(Guid id, out float totalDuration) => TotalDurations.TryGetValue(id, out totalDuration);

    public bool TryGetRemainingDuration(Guid id, out float remainingDuration) => Durations.TryGetValue(id, out remainingDuration);

    public void ForEach(Action<TEntity> action)
    {
        foreach (TEntity entity in Entities.Values)
        {
            action(entity);
        }
    }

    public Guid AddIndefinite(TEntity entity) => Add(entity);

    public Guid AddTemporary(TEntity entity, float duration) => Add(entity, duration);

    public void Remove(Guid id)
    {
        if (Entities.TryGetValue(id, out TEntity entity))
        {
            onEffectRemoved(id, entity);

            Entities.Remove(id);
            Durations.Remove(id);
            TotalDurations.Remove(id);
        }
    }

    public void Clear()
    {
        Entities.Clear();
        TotalDurations.Clear();
        Durations.Clear();
    }

    private Guid Add(TEntity entity, float? duration = null)
    {
        Guid id = Guid.NewGuid();

        Entities.Add(id, entity);

        if (duration.HasValue)
        {
            Durations.Add(id, duration.Value);
            TotalDurations.Add(id, duration.Value);
        }

        onEffectAdded(id, entity);

        return id;
    }

    private void TickDurations()
    {
        Durations.Keys.ToList().ForEach(id =>
        {
            Durations[id] -= Time.deltaTime;

            if (Durations[id] <= 0)
            {
                Remove(id);
            }
        });
    }
}