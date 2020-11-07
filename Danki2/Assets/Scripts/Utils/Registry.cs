using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Registry<TEntity>
{
    public Dictionary<Guid, TEntity> Entities { get; } = new Dictionary<Guid, TEntity>();

    public Dictionary<Guid, float> TotalDurations { get; } = new Dictionary<Guid, float>();

    public Dictionary<Guid, float> Durations { get; } = new Dictionary<Guid, float>();

    public Registry(Subject updateSubject)
    {
        updateSubject.Subscribe(TickDurations);
    }

    public Guid AddIndefinite(TEntity entity) => Add(entity);

    public Guid AddTemporary(TEntity entity, float duration)
    {
        Guid id = Add(entity);

        Durations.Add(id, duration);
        TotalDurations.Add(id, duration);

        return id;
    }

    public void Remove(Guid id)
    {
        // Note that Dicionary.Remove returns false if the key is not found.
        Entities.Remove(id);
        Durations.Remove(id);
        TotalDurations.Remove(id);
    }

    private Guid Add(TEntity entity)
    {
        Guid id = Guid.NewGuid();

        Entities.Add(id, entity);

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