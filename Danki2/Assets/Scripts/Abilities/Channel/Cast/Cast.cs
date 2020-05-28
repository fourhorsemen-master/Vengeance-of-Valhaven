using UnityEngine;

public abstract class Cast : Channel
{
    public sealed override ChannelType ChannelType => ChannelType.Cast;

    protected Cast(Actor owner, AbilityData abilityData, string[] availableBonuses)
        : base(owner, abilityData, availableBonuses)
    {
    }

    public sealed override void Start(Vector3 target) { }

    public sealed override void Continue(Vector3 target) { }

    public sealed override void Cancel(Vector3 target)
    {
        SuccessFeedbackSubject.Next(false);
    }
}