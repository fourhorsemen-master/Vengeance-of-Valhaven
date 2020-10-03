using System.Collections.Generic;
using UnityEngine;

public class RoomManager : Singleton<RoomManager>
{
    public List<ActorCacheItem> ActorCache { get; } = new List<ActorCacheItem>();
    public Player Player { get; private set; }
    
    private readonly Dictionary<int, List<WolfSpawner>> clusters = new Dictionary<int, List<WolfSpawner>>();

    private void Start()
    {
        Player = FindObjectOfType<Player>();
        TryAddToCache(Player);

        WolfSpawner[] spawners = FindObjectsOfType<WolfSpawner>();
        foreach (WolfSpawner spawner in spawners)
        {
            int clusterId = spawner.Cluster;

            if (clusters.TryGetValue(clusterId, out List<WolfSpawner> cluster))
            {
                cluster.Add(spawner);
                continue;
            }

            clusters[clusterId] = ListUtils.Singleton(spawner);
        }
    }

    public bool TryGetActor(GameObject gameObject, out Actor actor)
    {
        foreach(ActorCacheItem item in ActorCache)
        {
            if (item.Actor.gameObject.MatchesId(gameObject))
            {
                actor = item.Actor;
                return true;
            }
        }

        actor = null;
        return false;
    }

    public void TryAddToCache(Actor actor)
    {
        if (!actor.TryGetComponent(out Collider collider))
        {
            Debug.LogError($"Received actor, of type {actor.GetType()}, without a collider");
            return;
        }

        ActorCacheItem actorCacheItem = new ActorCacheItem(actor, collider);
        ActorCache.Add(actorCacheItem);
        actor.DeathSubject.Subscribe(() => ActorCache.Remove(actorCacheItem));
    }
}
