using UnityEngine;

public abstract class InstantCast : Ability
{
    protected InstantCast(Actor owner, AbilityData abilityData) : base(owner, abilityData)
    {
    }

    public abstract void Cast(Vector3 target);
}
