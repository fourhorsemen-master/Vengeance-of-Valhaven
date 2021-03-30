using UnityEngine;

public abstract class Charge : Channel
{
    public sealed override ChannelType ChannelType => ChannelType.Charge;

    protected float TimeCharged { get; private set; } = 0;

    protected Charge(AbilityConstructionArgs arguments) : base(arguments) { }

    public sealed override void Start(Vector3 floorTargetPosition, Vector3 offsetTargetPosition) => Start();

    public sealed override void Start(Actor actor) => Start();

    public sealed override void Continue(Vector3 floorTargetPosition, Vector3 offsetTargetPosition)
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