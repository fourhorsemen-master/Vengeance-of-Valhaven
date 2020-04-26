using UnityEngine;

public abstract class Channel : Ability
{
    public abstract float Duration { get; }

    protected Channel(Actor owner, AbilityData abilityData) : base(owner, abilityData)
    {
    }
    
    public virtual void Start(Vector3 target) { }

    public virtual void Continue(Vector3 target) { }

    public virtual void Cancel(Vector3 target) { }

    public virtual void End(Vector3 target) { }
}
