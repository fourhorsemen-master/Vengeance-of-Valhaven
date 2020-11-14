using System.Collections.Generic;
using UnityEngine;

public class HearsHowl : StateMachineTrigger
{
    private readonly Actor actor;
    private readonly float range;

    private readonly List<Subscription> howlSubscriptions = new List<Subscription>();

    private bool heardHowl;

    public HearsHowl(Actor actor, float range)
    {
        this.actor = actor;
        this.range = range;
    }

    public override void Activate()
    {
        heardHowl = false;
        
        RoomManager.Instance.ActorCache.ForEach(actorCacheItem =>
        {
            if (actorCacheItem.Actor.Equals(actor)) return;
            if (actorCacheItem.Actor.Type != ActorType.Wolf) return;

            Wolf wolf = (Wolf) actorCacheItem.Actor;
            howlSubscriptions.Add(wolf.OnHowl.Subscribe(() =>
            {
                heardHowl = heardHowl || Vector3.Distance(actor.transform.position, wolf.transform.position) <= range;
            }));
        });
    }

    public override void Deactivate()
    {
        howlSubscriptions.ForEach(s => s.Unsubscribe());
        howlSubscriptions.Clear();
    }

    public override bool Triggers()
    {
        return heardHowl;
    }
}
