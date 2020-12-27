using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomManager : Singleton<RoomManager>
{
    public List<ActorCacheItem> ActorCache { get; private set; }
    public Player Player { get; private set; }
    public bool CanAdvance { get; private set; } = false;

    public Subject CanAdvanceSubject = new Subject();

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
        ActorCache = actors
            .Select(actor =>
            {
                if (!actor.TryGetComponent(out Collider collider))
                {
                    Debug.LogError($"Found actor, of type {actor.GetType()}, without a collider");
                    return null;
                }

                if (actor.Type == ActorType.Player)
                {
                    Player = (Player)actor;
                }

                return new ActorCacheItem(actor, collider);
            })
            .Where(i => i != null)
            .ToList();

        ActorCache.ForEach(a =>
            a.Actor.DeathSubject.Subscribe(() =>
            {
                ActorCache.Remove(a);

                if (!ActorCache.Any(b => b.Actor.CompareTag(Tags.Enemy)))
                {
                    if (!CanAdvance)
                    {
                        CanAdvanceSubject.Next();
                        CanAdvance = true;
                    }
                }
            })
        );
    }
}
