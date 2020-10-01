using System.Collections.Generic;
using UnityEngine;

public class RoomManager : Singleton<RoomManager>
{
    public List<ActorCacheItem> ActorCache { get; } = new List<ActorCacheItem>();
    public Player Player { get; private set; }

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

    private void Start()
    {
        Actor[] actors = FindObjectsOfType<Actor>();
        foreach (Actor actor in actors)
        {
            TryAddToCache(actor);

            if (actor.Type == ActorType.Player)
            {
                Player = (Player)actor;
            }
        }
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
