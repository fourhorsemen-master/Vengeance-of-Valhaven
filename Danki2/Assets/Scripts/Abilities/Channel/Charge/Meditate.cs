using System;
using System.Collections.Generic;
using UnityEngine;

[Ability(AbilityReference.Meditate, new []{"Clarity", "GrowingRage"})]
public class Meditate : Charge
{
    private readonly Dictionary<Actor, Guid> slowedActors = new Dictionary<Actor, Guid>();

    private MeditateObject meditateObject;

    private const float PowerDuration = 10;
    private const int GrowingRageMultiplier = 2;
    private const float SlowMultiplier = 0.5f;

    public Meditate(Actor owner, AbilityData abilityData, string[] availableBonuses, float duration)
        : base(owner, abilityData, availableBonuses, duration)
    {
    }

    protected override void Start()
    {
        meditateObject = MeditateObject.Create(Owner.transform, Duration);
        
        if (HasBonus("Clarity")) AddSlowEffects();
    }

    protected override void Continue()
    {
        if (GetPowerIncrease() > 0) SuccessFeedbackSubject.Next(true);
    }

    public override void End(Vector3 floorTargetPosition, Vector3 offsetTargetPosition) => End();

    public override void End(Actor actor) => End();

    public override void Cancel(Vector3 floorTargetPosition, Vector3 offsetTargetPosition) => End();

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
        
        // Just commenting out as this ability will be removed later anyway
        // LinearStatModification powerModification = new PowerBuff(powerIncrease);
        // Owner.EffectManager.AddActiveEffect(powerModification, PowerDuration);
    }

    private void AddSlowEffects()
    {
        RoomManager.Instance.ActorCache.ForEach(actorCacheItem =>
        {
            Actor actor = actorCacheItem.Actor;

            if (actor.Opposes(Owner))
            {
                // Just commenting out as this ability will be removed later anyway
                // MultiplicativeStatModification slow = new Slow(SlowMultiplier);

                // if (actor.EffectManager.TryAddPassiveEffect(slow, out Guid slowEffectId))
                // {
                //     slowedActors[actor] = slowEffectId;
                // }
            }
        });
    }

    private void RemoveSlowEffects()
    {
        foreach (KeyValuePair<Actor, Guid> keyValuePair in slowedActors)
        {
            Actor actor = keyValuePair.Key;
            Guid effectId = keyValuePair.Value;
            
            // Just commenting out as this ability will be removed later anyway
            // actor.EffectManager.RemoveEffect(effectId);
        }
    }

    private int GetPowerIncrease()
    {
        return HasBonus("GrowingRage")
            ? Mathf.FloorToInt(TimeCharged) * GrowingRageMultiplier
            : Mathf.FloorToInt(TimeCharged);
    }
}
