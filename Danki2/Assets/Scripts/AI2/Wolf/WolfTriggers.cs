using System.Collections.Generic;
using UnityEngine;

public class IfRandomTimeElapsed : IAiTrigger
{
    private readonly float minTime;
    private readonly float maxTime;

    private float requiredTime;

    public IfRandomTimeElapsed(float minTime, float maxTime)
    {
        this.minTime = minTime;
        this.maxTime = maxTime;
    }

    public void Activate()
    {
        requiredTime = Time.time + Random.Range(minTime, maxTime);
    }

    public void Deactivate() {}

    public bool Triggers()
    {
        return Time.time >= requiredTime;
    }
}

public class IfTimeElapsed : IAiTrigger
{
    private readonly float time;

    private float requiredTime;

    public IfTimeElapsed(float time)
    {
        this.time = time;
    }

    public void Activate()
    {
        requiredTime = Time.time + time;
    }

    public void Deactivate() {}

    public bool Triggers()
    {
        return Time.time >= requiredTime;
    }
}

public class IfDistanceLessThan : IAiTrigger
{
    private readonly Actor actor1;
    private readonly Actor actor2;
    private readonly float distance;

    public IfDistanceLessThan(Actor actor1, Actor actor2, float distance)
    {
        this.actor1 = actor1;
        this.actor2 = actor2;
        this.distance = distance;
    }

    public void Activate() {}

    public void Deactivate() {}

    public bool Triggers()
    {
        return Vector3.Distance(actor1.transform.position, actor2.transform.position) < distance;
    }
}

public class IfDistanceGreaterThan : IAiTrigger
{
    private readonly Actor actor1;
    private readonly Actor actor2;
    private readonly float distance;

    public IfDistanceGreaterThan(Actor actor1, Actor actor2, float distance)
    {
        this.actor1 = actor1;
        this.actor2 = actor2;
        this.distance = distance;
    }

    public void Activate() {}

    public void Deactivate() {}

    public bool Triggers()
    {
        return Vector3.Distance(actor1.transform.position, actor2.transform.position) > distance;
    }
}

public class IfHeardHowl : IAiTrigger
{
    private readonly Actor actor;
    private readonly float range;

    private readonly List<Subscription> howlSubscriptions = new List<Subscription>();

    private bool heardHowl;

    public IfHeardHowl(Actor actor, float range)
    {
        this.actor = actor;
        this.range = range;
    }

    public void Activate()
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

    public void Deactivate()
    {
        howlSubscriptions.ForEach(s => s.Unsubscribe());
        howlSubscriptions.Clear();
    }

    public bool Triggers()
    {
        return heardHowl;
    }
}

public class IfTakenDamage : IAiTrigger
{
    private readonly Actor actor;

    private bool takenDamage;
    private Subscription damageSubscription;

    public IfTakenDamage(Actor actor)
    {
        this.actor = actor;
    }

    public void Activate()
    {
        takenDamage = false;

        damageSubscription = actor.HealthManager.DamageSubject
            .Subscribe(() => takenDamage = true);
    }

    public void Deactivate()
    {
        damageSubscription.Unsubscribe();
    }

    public bool Triggers()
    {
        return takenDamage;
    }
}

public class IfHealthGoesLessThan : IAiTrigger
{
    private readonly Actor actor;
    private readonly int threshold;
    
    private bool canTrigger;

    public IfHealthGoesLessThan(Actor actor, int threshold)
    {
        this.actor = actor;
        this.threshold = threshold;
    }

    public void Activate()
    {
        canTrigger = actor.HealthManager.Health >= threshold;
    }

    public void Deactivate() {}

    public bool Triggers()
    {
        return canTrigger && actor.HealthManager.Health < threshold;
    }
}

public class IfAnything : IAiTrigger
{
    public void Activate() {}
    public void Deactivate() {}
    public bool Triggers() => true;
}

public class IfRandomWolfAttackCount : IAiTrigger
{
    private readonly Wolf wolf;
    private readonly int minAttacks;
    private readonly int maxAttacks;

    private int attacks;
    private int requiredAttacks;
    private Subscription attackSubscription;

    public IfRandomWolfAttackCount(Wolf wolf, int minAttacks, int maxAttacks)
    {
        this.wolf = wolf;
        this.minAttacks = minAttacks;
        this.maxAttacks = maxAttacks;
    }

    public void Activate()
    {
        attacks = 0;
        requiredAttacks = Random.Range(minAttacks, maxAttacks + 1);
        attackSubscription = wolf.OnAttack.Subscribe(() => attacks++);
    }

    public void Deactivate()
    {
        attackSubscription.Unsubscribe();
    }

    public bool Triggers()
    {
        return attacks >= requiredAttacks;
    }
}
