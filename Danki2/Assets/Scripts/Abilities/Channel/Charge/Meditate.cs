using System;
using System.Collections.Generic;
using UnityEngine;

[Ability(AbilityReference.Meditate, new []{"Clarity", "GrowingRage"})]
public class Meditate : Charge
{
    protected override float ChargeTime => 5;
    
    private readonly Dictionary<Actor, Guid> slowedActors = new Dictionary<Actor, Guid>();

    private MeditateObject meditateObject;

    private const float PowerDuration = 10;
    private const int GrowingRageMultiplier = 2;
    private const float SlowMultiplier = 0.5f;

    public Meditate(Actor owner, AbilityData abilityData, string[] availableBonuses) : base(owner, abilityData, availableBonuses)
    {
    }

    protected override void Start()
    {
        meditateObject = MeditateObject.Create(Owner.transform, ChargeTime);

        Owner.DeathSubject.Subscribe(meditateObject.Destroy);
        
        if (HasBonus("Clarity")) AddSlowEffects();
    }

    protected override void Continue()
    {
        if (GetPowerIncrease() > 0) SuccessFeedbackSubject.Next(true);
    }

    public override void End(Vector3 target) => End();

    public override void End(Actor actor) => End();

    public override void Cancel(Vector3 target) => End();

    public override void Cancel(Actor actor) => End();

    private void End()
    {
        meditateObject.Destroy();
        
        RemoveSlowEffects();
        
        int powerIncrease = GetPowerIncrease();

        if (powerIncrease == 0)
        {
            SuccessFeedbackSubject.Next(false);
            return;
        }
        
        LinearStatModification powerModification = new PowerBuff(powerIncrease);
        Owner.EffectManager.AddActiveEffect(powerModification, PowerDuration);
    }

    private void AddSlowEffects()
    {
        RoomManager.Instance.ActorCache.ForEach(actorCacheItem =>
        {
            Actor actor = actorCacheItem.Actor;

            if (actor.Opposes(Owner))
            {
                MultiplicativeStatModification slow = new Slow(SlowMultiplier);
                slowedActors[actor] = actor.EffectManager.AddPassiveEffect(slow);
            }
        });
    }

    private void RemoveSlowEffects()
    {
        foreach (KeyValuePair<Actor, Guid> keyValuePair in slowedActors)
        {
            Actor actor = keyValuePair.Key;
            Guid effectId = keyValuePair.Value;
            
            actor.EffectManager.RemovePassiveEffect(effectId);
        }
    }

    private int GetPowerIncrease()
    {
        return HasBonus("GrowingRage")
            ? Mathf.FloorToInt(TimeCharged) * GrowingRageMultiplier
            : Mathf.FloorToInt(TimeCharged);
    }
}
