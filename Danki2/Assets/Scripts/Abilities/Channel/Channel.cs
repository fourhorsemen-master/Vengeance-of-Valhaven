using UnityEngine;

public abstract class Channel : Ability
{
    public abstract float Duration { get; }

    protected Channel(Actor owner, AbilityData abilityData, string[] availableBonuses) : base(owner, abilityData, availableBonuses)
    {
    }
    
    public virtual void Start(Vector3 target) { }

    public virtual void Continue(Vector3 target) { }

    public virtual void Cancel(Vector3 target) { }

    public virtual void End(Vector3 target) { }
}
