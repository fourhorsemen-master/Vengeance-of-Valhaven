using UnityEngine;

public abstract class InstantCast : Ability
{
    protected InstantCast(AbilityConstructionArgs arguments) : base(arguments) { }

    public abstract void Cast(Vector3 floorTargetPosition, Vector3 offsetTargetPosition);

    public virtual void Cast(Actor target) => Cast(target.transform.position, target.Centre);
}
