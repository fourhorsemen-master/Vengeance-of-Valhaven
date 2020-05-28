using UnityEngine;

public abstract class Charge : Channel
{
    public sealed override ChannelType ChannelType => ChannelType.Charge;
    public sealed override float Duration => ChargeTime;

    protected abstract float ChargeTime { get; }

    protected Charge(Actor owner, AbilityData abilityData, string[] availableBonuses)
        : base(owner, abilityData, availableBonuses)
    {
    }

    public sealed override void Start(Vector3 target) { }

    public sealed override void Continue(Vector3 target) { }
}