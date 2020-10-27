using System.Collections.Generic;
using UnityEngine;

public class IfRandomTimeElapsed : IAiTrigger
{
    private readonly float minTime;
    private readonly float maxTime;

    private float startTime;
    private float requiredTime;

    public IfRandomTimeElapsed(float minTime, float maxTime)
    {
        this.minTime = minTime;
        this.maxTime = maxTime;
    }

    public void Activate()
    {
        startTime = Time.time;
        requiredTime = Random.Range(minTime, maxTime + 1);
    }

    public bool Triggers()
    {
        return Time.time - startTime >= requiredTime;
    }

    public void Deactivate() {}
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

    public bool Triggers()
    {
        return Vector3.Distance(actor1.Centre, actor2.Centre) < distance;
    }

    public void Deactivate() {}
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

    public bool Triggers()
    {
        return Vector3.Distance(actor1.Centre, actor2.Centre) > distance;
    }

    public void Deactivate() {}
}

public class IfTimeGreaterThan : IAiTrigger
{
    private readonly float requiredTime;

    private float startTime;

    public IfTimeGreaterThan(float requiredTime)
    {
        this.requiredTime = requiredTime;
    }

    public void Activate()
    {
        startTime = Time.time;
    }

    public bool Triggers()
    {
        return Time.time - startTime >= requiredTime;
    }

    public void Deactivate() {}
}

public class IfHearsHowl : IAiTrigger
{
    private readonly Actor actor;
    private readonly float range;

    private readonly List<Subscription<Wolf>> howlSubscriptions = new List<Subscription<Wolf>>();

    private bool heardHowl = false;

    public IfHearsHowl(Actor actor, float range)
    {
        this.actor = actor;
        this.range = range;
    }

    public void Activate()
    {
        RoomManager.Instance.ActorCache.ForEach(actorCacheItem =>
        {
            if (actorCacheItem.Actor.Equals(actor)) return;
            if (actorCacheItem.Actor.Type != ActorType.Wolf) return;

            howlSubscriptions.Add(((Wolf) actorCacheItem.Actor).OnHowl.Subscribe(wolf =>
            {
                heardHowl = heardHowl || Vector3.Distance(actor.Centre, wolf.Centre) <= range;
            }));
        });
    }

    public bool Triggers()
    {
        return heardHowl;
    }

    public void Deactivate()
    {
        howlSubscriptions.ForEach(s => s.Unsubscribe());
        howlSubscriptions.Clear();
        heardHowl = false;
    }
}

public class IfTakesDamage : IAiTrigger
{
    private readonly Actor actor;
    
    private Subscription damageSubscription;

    private bool takenDamage = false;

    public IfTakesDamage(Actor actor)
    {
        this.actor = actor;
    }

    public void Activate()
    {
        damageSubscription = actor.HealthManager.DamageSubject
            .Subscribe(() => takenDamage = true);
    }

    public bool Triggers()
    {
        return takenDamage;
    }

    public void Deactivate()
    {
        damageSubscription.Unsubscribe();
        takenDamage = false;
    }
}

public class IfHealthGoesLessThan : IAiTrigger
{
    private readonly Actor actor;
    private readonly int threshold;
    
    private int initialHealth;
    private bool canTrigger;

    public IfHealthGoesLessThan(Actor actor, int threshold)
    {
        this.actor = actor;
        this.threshold = threshold;
    }

    public void Activate()
    {
        initialHealth = actor.HealthManager.Health;
        canTrigger = initialHealth >= threshold;
    }

    public bool Triggers()
    {
        return canTrigger && actor.HealthManager.Health < threshold;
    }

    public void Deactivate() {}
}

public class InstantTrigger : IAiTrigger
{
    public void Activate() {}
    public bool Triggers() => true;
    public void Deactivate() {}
}

public class IfAttacksGreaterThanRandom : IAiTrigger
{
    private readonly Wolf wolf;
    private readonly int minAttacks;
    private readonly int maxAttacks;

    private int requiredAttacks;
    private int attacks = 0;
    private Subscription attackSubscription;

    public IfAttacksGreaterThanRandom(Wolf wolf, int minAttacks, int maxAttacks)
    {
        this.wolf = wolf;
        this.minAttacks = minAttacks;
        this.maxAttacks = maxAttacks;
    }

    public void Activate()
    {
        requiredAttacks = Random.Range(minAttacks, maxAttacks + 1);
        attacks = 0;
        attackSubscription = wolf.OnAttack.Subscribe(() => attacks++);
    }

    public bool Triggers()
    {
        return attacks >= requiredAttacks;
    }

    public void Deactivate()
    {
        attackSubscription.Unsubscribe();
    }
}
