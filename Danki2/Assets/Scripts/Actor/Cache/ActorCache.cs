using System.Collections.Generic;
using UnityEngine;

public class ActorCache : Singleton<ActorCache>
{
    public List<ActorCacheItem> Cache { get; } = new List<ActorCacheItem>();
    public Player Player { get; private set; }

    public bool TryGetActor(GameObject gameObject, out Actor actor)
    {
        foreach(ActorCacheItem item in Cache)
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

    private void Start()
    {
        if (Player == null)
        {
            Debug.LogError("No player registered with the actor cache");
        }
    }

    public void Register(Actor actor)
    {
        if (!actor.TryGetComponent(out Collider collider))
        {
            Debug.LogError($"Found actor, of type {actor.GetType()}, without a collider");
            return;
        }

        if (actor.Type == ActorType.Player)
        {
            if (Player != null)
            {
                Debug.LogError("Tried to register more than one player with the actor cache");
                return;
            }
            Player = (Player)actor;
        }

        ActorCacheItem actorCacheItem = new ActorCacheItem(actor, collider);
        Cache.Add(actorCacheItem);
        actorCacheItem.Actor.DeathSubject.Subscribe(_ => Cache.Remove(actorCacheItem));
    }
}
