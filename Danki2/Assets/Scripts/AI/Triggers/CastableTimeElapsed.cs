using UnityEngine;

public class CastableTimeElapsed : StateMachineTrigger
{
    private readonly Actor actor;
    private readonly float timePeriod;

    private float requiredTime;

    public CastableTimeElapsed(Actor actor, float timePeriod)
    {
        this.actor = actor;
        this.timePeriod = timePeriod;
    }

    public override void Activate()
    {
        requiredTime = Time.time + timePeriod;
    }

    public override void Deactivate() { }

    public override bool Triggers()
    {
        // TODO: Bring in stunlocks for enemies. Until then all time is castable time.
        return Time.time >= requiredTime;

        //For reference we did this before:
        //  return Time.time >= requiredTime && actor.CastableTimeElapsed >= timePeriod;
    }
}
