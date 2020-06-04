using UnityEngine;

public abstract class InstantCast : Ability
{
    protected InstantCast(Actor owner, AbilityData abilityData, string[] availableBonuses) : base(owner, abilityData, availableBonuses)
    {
    }

    public abstract void Cast(Vector3 target);

    public virtual void Cast(Actor actor) => Cast(actor.Centre);
}
