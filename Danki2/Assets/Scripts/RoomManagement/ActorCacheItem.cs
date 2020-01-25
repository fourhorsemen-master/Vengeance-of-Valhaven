using UnityEngine;

public class ActorCacheItem
{
    public Actor Actor { get; }
    public Collider Collider { get; }

    public ActorCacheItem(Actor actor, Collider collider)
    {
        Actor = actor;
        Collider = collider;
    }
}
