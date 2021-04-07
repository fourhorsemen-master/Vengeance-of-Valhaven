using System;
using UnityEngine;

[Ability(AbilityReference.Parry)]
public class Parry : Channel
{
    private const float reflectedDamageProportion = 0.5f;
    
    private Guid damagePipeId;
    private readonly Subject onParry = new Subject();

    public Parry(AbilityConstructionArgs arguments) : base(arguments) { }

    public override void Start(Vector3 floorTargetPosition, Vector3 offsetTargetPosition)
    {
        ParryObject.Create(Owner.transform, onParry);

        damagePipeId = Owner.HealthManager.RegisterDamagePipe(DamagePipe);
    }

    public override void Cancel(Vector3 floorTargetPosition, Vector3 offsetTargetPosition) => Finish();

    public override void End(Vector3 floorTargetPosition, Vector3 offsetTargetPosition) => Finish();

    private bool DamagePipe(DamageData damageData)
    {
        onParry.Next();
        
        SuccessFeedbackSubject.Next(true);

        int damage = Mathf.FloorToInt(damageData.Damage * reflectedDamageProportion);
        DealPrimaryDamage(damageData.Source, damage);

        return false;
    }

    private void Finish()
    {
        Owner.HealthManager.DeregisterDamagePipe(damagePipeId);
        SuccessFeedbackSubject.Next(false);
    }
}
