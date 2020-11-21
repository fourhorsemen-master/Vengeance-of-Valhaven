using UnityEngine;

public abstract class Cast : Channel
{
    public sealed override ChannelType ChannelType => ChannelType.Cast;

    protected Cast(Actor owner, AbilityData abilityData, string[] availableBonuses, float duration)
        : base(owner, abilityData, availableBonuses, duration)
    {
    }

    public sealed override void Start(Vector3 floorTargetPosition, Vector3 offsetTargetPosition) => Start();

    public sealed override void Start(Actor actor) => Start();

    public sealed override void Continue(Vector3 floorTargetPosition, Vector3 offsetTargetPosition) => Continue();

    public sealed override void Continue(Actor actor) => Continue();

    public sealed override void Cancel(Vector3 floorTargetPosition, Vector3 offsetTargetPosition)
    {
        SuccessFeedbackSubject.Next(false);
        Cancel();
    }

    public sealed override void Cancel(Actor actor)
    {
        SuccessFeedbackSubject.Next(false);
        Cancel();
    }
    
    protected virtual void Start() { }
    
    protected virtual void Continue() { }
    
    protected virtual void Cancel() { }
}