using UnityEngine;

public abstract class Charge : Channel
{
    public sealed override ChannelType ChannelType => ChannelType.Charge;
    public sealed override float Duration => ChargeTime;

    protected abstract float ChargeTime { get; }

    protected float TimeCharged { get; private set; } = 0;

    protected Charge(Actor owner, AbilityData abilityData, string[] availableBonuses)
        : base(owner, abilityData, availableBonuses)
    {
    }

    public sealed override void Start(Vector3 target) => Start();

    public sealed override void Start(Actor actor) => Start();

    public sealed override void Continue(Vector3 target)
    {
        TickTimeCharged();
        Continue();
    }

    public sealed override void Continue(Actor actor)
    {
        TickTimeCharged();
        Continue();
    }

    protected virtual void Start() { }

    protected virtual void Continue() { }

    private void TickTimeCharged() => TimeCharged += Time.deltaTime;
}