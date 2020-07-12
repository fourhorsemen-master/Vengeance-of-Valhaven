using System.Collections.Generic;
using UnityEngine;

[Ability(AbilityReference.Parry)]
public class Parry : InstantCast
{
    private const float duration = 1f;
    private const float reflectedDamagePercent = 0.5f;
    
    private Subscription<DamageData> damageSourceSubscription;
    private readonly Dictionary<Actor, int> damageSourceToAmount = new Dictionary<Actor, int>();
    private ParryObject parryObject;

    private bool ReceivedDamage => damageSourceToAmount.Count > 0;
    
    public Parry(Actor owner, AbilityData abilityData, string[] availableBonuses) : base(owner, abilityData, availableBonuses)
    {
    }

    public override void Cast(Vector3 target)
    {
        parryObject = ParryObject.Create(Owner.transform);
        
        Owner.EffectManager.AddActiveEffect(new BlockIncomingDamage(), duration);
        
        damageSourceSubscription = Owner.HealthManager.UnmodifiedDamageSubject.Subscribe(damageData =>
        {
            if (!ReceivedDamage) SuccessFeedbackSubject.Next(true);
            IncrementDamage(damageData);
        });

        Coroutine finishCoroutine = Owner.WaitAndAct(duration, Finish);
        Owner.DeathSubject.Subscribe(() =>
        {
            Owner.StopCoroutine(finishCoroutine);
            damageSourceSubscription.Unsubscribe();
        });
    }

    private void IncrementDamage(DamageData damageData)
    {
        if (damageSourceToAmount.ContainsKey(damageData.Source))
        {
            damageSourceToAmount[damageData.Source] += damageData.Damage;
        }
        else
        {
            damageSourceToAmount[damageData.Source] = damageData.Damage;
        }
    }

    private void Finish()
    {
        damageSourceSubscription.Unsubscribe();
        if (ReceivedDamage)
        {
            CustomCamera.Instance.AddShake(ShakeIntensity.Medium);
            DamageAttackers();
        }
        else
        {
            SuccessFeedbackSubject.Next(false);
        }
    }

    private void DamageAttackers()
    {
        foreach (KeyValuePair<Actor,int> keyValuePair in damageSourceToAmount)
        {
            Actor source = keyValuePair.Key;
            int damage = Mathf.FloorToInt(keyValuePair.Value * reflectedDamagePercent);
            
            DealPrimaryDamage(source, damage);
        }
    }
}
