using UnityEngine;

public abstract class Charge : Channel
{
    public sealed override ChannelType ChannelType => ChannelType.Charge;

    protected float TimeCharged { get; private set; } = 0;

    protected Charge(Actor owner, AbilityData abilityData, string[] availableBonuses, float duration)
        : base(owner, abilityData, availableBonuses, duration)
    {
    }

    public sealed override void Start(Vector3 _, Vector3 __) => Start();

    public sealed override void Start(Actor actor) => Start();

    public sealed override void Continue(Vector3 _, Vector3 __)
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