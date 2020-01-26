﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomManager : Singleton<RoomManager>
{
    public List<ActorCacheItem> ActorCache { get; private set; }

    private void Start()
    {
        Actor[] actors = FindObjectsOfType<Actor>();
        ActorCache = actors
            .Select(actor =>
            {
                if (!actor.TryGetComponent(out Collider collider))
                {
                    Debug.LogError($"Found actor, of type {actor.GetType()}, without a collider");
                    return null;
                }

                return new ActorCacheItem(actor, collider);
            })
            .Where(i => i != null)
            .ToList();
    }
}
