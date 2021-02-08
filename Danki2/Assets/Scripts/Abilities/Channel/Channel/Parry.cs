using System;
using UnityEngine;

[Ability(AbilityReference.Parry)]
public class Parry : Channel
{
    private const float reflectedDamageProportion = 0.5f;
    
    private Subscription<DamageData> damageSourceSubscription;
    private readonly Subject onParry = new Subject();

    private bool receivedDamage = false;
    private Guid effectId;

    public Parry(Actor owner, AbilityData abilityData, string fmodStartEvent, string fmodEndEvent, string[] availableBonuses, float duration)
        : base(owner, abilityData, fmodStartEvent, fmodEndEvent, availableBonuses, duration)
    {
    }

    public override void Start(Vector3 floorTargetPosition, Vector3 offsetTargetPosition)
    {
        ParryObject.Create(Owner.transform, onParry);
        
        Owner.EffectManager.TryAddPassiveEffect(PassiveEffect.Block, out effectId);
        
        damageSourceSubscription = Owner.HealthManager.UnmodifiedDamageSubject.Subscribe(HandleIncomingDamage);
    }

    public override void Cancel(Vector3 floorTargetPosition, Vector3 offsetTargetPosition) => Finish();

    public override void End(Vector3 floorTargetPosition, Vector3 offsetTargetPosition) => Finish();

    private void HandleIncomingDamage(DamageData damageData)
    {
        onParry.Next();
        
        if (!receivedDamage) SuccessFeedbackSubject.Next(true);
        receivedDamage = true;

        int damage = Mathf.FloorToInt(damageData.Damage * reflectedDamageProportion);
        DealPrimaryDamage(damageData.Source, damage);
    }

    private void Finish()
    {
        Owner.EffectManager.RemovePassiveEffect(effectId);
        damageSourceSubscription.Unsubscribe();
        if (!receivedDamage) SuccessFeedbackSubject.Next(false);
    }
}
