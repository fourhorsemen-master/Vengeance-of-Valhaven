using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class RoomManager : Singleton<RoomManager>
{
    private const float MinClusterDistanceFromPlayer = 10f;
    
    public List<ActorCacheItem> ActorCache { get; } = new List<ActorCacheItem>();
    public Player Player { get; private set; }
    public Subject<int> WaveStartSubject { get; } = new Subject<int>();
    
    private readonly Dictionary<int, Cluster> clusters = new Dictionary<int, Cluster>();
    private int wave = 0;

    private void Start()
    {
        SetupGameObjectReferences();
        StartNextWave();
    }

    private void Update()
    {
        TryStartNextWave();
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

    private void SetupGameObjectReferences()
    {
        Player = FindObjectOfType<Player>();
        TryAddToCache(Player);

        WolfSpawner[] spawners = FindObjectsOfType<WolfSpawner>();
        foreach (WolfSpawner spawner in spawners)
        {
            int clusterId = spawner.Cluster;

            if (clusters.TryGetValue(clusterId, out Cluster cluster))
            {
                cluster.AddSpawner(spawner);
                continue;
            }

            clusters[clusterId] = new Cluster(spawner);
        }
    }

    private void TryStartNextWave()
    {
        if (ActorCache.Count == 1 && ActorCache[0].Actor.Type == ActorType.Player)
        {
            StartNextWave();
        }
    }

    private void StartNextWave()
    {
        wave++;
        clusters[SelectCluster()].Spawn(wave);
        WaveStartSubject.Next(wave);
    }

    private int SelectCluster()
    {
        List<int> potentialClusterIds = new List<int>();
        
        foreach (KeyValuePair<int, Cluster> keyValuePair in clusters)
        {
            Vector3 averagePosition = keyValuePair.Value.GetAveragePosition();
            
            if (Vector3.Distance(Player.transform.position, averagePosition) >= MinClusterDistanceFromPlayer)
            {
                potentialClusterIds.Add(keyValuePair.Key);
            }
        }
        
        return potentialClusterIds.Count == 0
            ? clusters.Keys.First()
            : potentialClusterIds[Random.Range(0, potentialClusterIds.Count)];
    }
}
