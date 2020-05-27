using UnityEngine;

public abstract class Charge : Channel
{
    public override ChannelType ChannelType => ChannelType.Charge;

    protected Charge(Actor owner, AbilityData abilityData, string[] availableBonuses)
        : base(owner, abilityData, availableBonuses)
    {
    }

    public sealed override void Start(Vector3 target) { }

    public sealed override void Continue(Vector3 target) { }
}