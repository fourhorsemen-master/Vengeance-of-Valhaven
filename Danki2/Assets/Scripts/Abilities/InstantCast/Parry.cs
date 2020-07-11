using System;
using System.Collections.Generic;
using UnityEngine;

[Ability(AbilityReference.Parry)]
public class Parry : InstantCast
{
    private const float waitTime = 1f;
    private const float damagePercent = 0.5f;
    
    private Subscription<Tuple<int, Actor>> damageSourceSubscription;
    private readonly Dictionary<Actor, int> damageSourceToAmount = new Dictionary<Actor, int>();

    private bool receivedDamage = false;
    
    public Parry(Actor owner, AbilityData abilityData, string[] availableBonuses) : base(owner, abilityData, availableBonuses)
    {
    }

    public override void Cast(Vector3 target)
    {
        damageSourceSubscription = Owner.HealthManager.DamageSourceSubject.Subscribe(tuple =>
        {
            (int damage, Actor source) = tuple;

            if (damageSourceToAmount.ContainsKey(source))
            {
                damageSourceToAmount[source] += damage;
            }
            else
            {
                damageSourceToAmount[source] = damage;
            }

            if (!receivedDamage) SuccessFeedbackSubject.Next(true);
            receivedDamage = true;
        });

        Owner.WaitAndAct(waitTime, () =>
        {
            damageSourceSubscription.Unsubscribe();
            if (receivedDamage)
            {
                DamageAttackers();
            }
            else
            {
                SuccessFeedbackSubject.Next(false);
            }
        });
    }

    private void DamageAttackers()
    {
        foreach (KeyValuePair<Actor,int> keyValuePair in damageSourceToAmount)
        {
            Actor source = keyValuePair.Key;
            int damage = Mathf.FloorToInt(keyValuePair.Value * damagePercent);
            
            DealPrimaryDamage(source, damage);
        }
    }
}
