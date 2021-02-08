using UnityEngine;

public abstract class InstantCast : Ability
{
    protected InstantCast(Actor owner, AbilityData abilityData, string fmodStartEvent, string fmodEndEvent, string[] availableBonuses)
        : base(owner, abilityData, fmodStartEvent, fmodEndEvent, availableBonuses)
    {
    }

    public abstract void Cast(Vector3 floorTargetPosition, Vector3 offsetTargetPosition);

    public virtual void Cast(Actor target) => Cast(target.transform.position, target.Centre);
}
