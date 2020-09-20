using UnityEngine;

[Ability(AbilityReference.Parry)]
public class Parry : InstantCast
{
    private const float duration = 1f;
    private const float reflectedDamageProportion = 0.5f;
    
    private Subscription<DamageData> damageSourceSubscription;
    private readonly Subject onParry = new Subject();

    private bool receivedDamage = false;
    
    public Parry(Actor owner, AbilityData abilityData, string[] availableBonuses) : base(owner, abilityData, availableBonuses)
    {
    }

    public override void Cast(Vector3 target)
    {
        ParryObject.Create(Owner.transform, onParry);
        
        Owner.EffectManager.AddActiveEffect(new BlockIncomingDamage(), duration);
        
        damageSourceSubscription = Owner.HealthManager.UnmodifiedDamageSubject.Subscribe(HandleIncomingDamage);

        Coroutine finishCoroutine = Owner.WaitAndAct(duration, Finish);

        Owner.DeathSubject.Subscribe(() =>
        {
            Owner.StopCoroutine(finishCoroutine);
            damageSourceSubscription.Unsubscribe();
        });
    }

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
        damageSourceSubscription.Unsubscribe();
        if (!receivedDamage) SuccessFeedbackSubject.Next(false);
    }
}
