using UnityEngine;

public class WraithCastBlink : IStateMachineComponent
{
    private readonly PositionFinder positionFinder;
    private readonly Wraith wraith;

    public WraithCastBlink(Wraith wraith, Actor actorToAvoid, float minDistance, float maxDistance)
    {
        this.wraith = wraith;

        positionFinder = new PositionFinder(wraith, actorToAvoid, minDistance, maxDistance);
    }

    public void Enter()
    {
        Vector3 blinkTo = positionFinder.GetRandomPositionAroundTarget();

        wraith.Blink(blinkTo);
    }

    public void Exit() {}
    public void Update() {}
}
