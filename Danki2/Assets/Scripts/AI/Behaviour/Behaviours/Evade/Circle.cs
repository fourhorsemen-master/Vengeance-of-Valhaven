using UnityEngine;

/// <summary>
/// With circle, we configure a ring around the target that we want to circle in.
/// Within the ring, we path clockwise around the player if possible, otherwise we switch to anti-clockwise circling.
/// Outside the ring, we path towward or away from the player until we are within the ring.
/// </summary>
[Behaviour("Circle target", new string[] { "Max circle distance", "Min circle distance" }, new AIAction[] { AIAction.Evade })]
public class Circle : Behaviour
{
    private float maxCircleDistance;
    private float minCircleDistance;
    private float favouredCircleDistance = float.NaN;
    private CirclePhase phase = CirclePhase.CirclingAnticlockwise;

    public override void Initialize()
    {
        maxCircleDistance = Args[0];
        minCircleDistance = Args[1];

        if (minCircleDistance > maxCircleDistance)
        {
            Debug.LogError("Min circle distance is greater than max circle distance.");
        }
    }

    public override void Behave(Actor actor)
    {
        if (!actor.Target)
        {
            return;
        }

        // We get nasty errors if we try to use Random methods in Initialize, se we initialise here.
        if (float.IsNaN(favouredCircleDistance))
        {
            favouredCircleDistance = Random.Range(minCircleDistance, maxCircleDistance);
        }

        Vector3 position = actor.transform.position;
        Vector3 target = actor.Target.transform.position;

        float distanceToTarget = Vector3.Distance(position, target);

        // Handle moving in
        if (
            distanceToTarget > maxCircleDistance
            || distanceToTarget > favouredCircleDistance && phase == CirclePhase.MovingIn
        )
        {
            phase = CirclePhase.MovingIn;
            actor.MovementManager.StartPathfinding(target);
            return;
        }

        // Handle moving out
        if (
            distanceToTarget < minCircleDistance
            || distanceToTarget < favouredCircleDistance && phase == CirclePhase.MovingOut
        )
        {
            Vector3 awayFromTarget = position + (position - target).normalized;

            if (actor.MovementManager.TryStartPathfinding(awayFromTarget))
            {
                phase = CirclePhase.MovingOut;
                return;
            }
        }

        // Handle circling
        if (phase != CirclePhase.CirclingAnticlockwise && phase != CirclePhase.CirclingClockwise)
        {
            phase = CirclePhase.CirclingClockwise;
        }

        Vector3 clockwiseDirection = Vector3.Cross(Vector3.up, target - position).normalized;
        Vector3 movementDirection = phase == CirclePhase.CirclingClockwise
            ? clockwiseDirection
            : -clockwiseDirection;

        Vector3 destination = position + movementDirection;

        if (!actor.MovementManager.TryStartPathfinding(destination))
        {
            phase = phase == CirclePhase.CirclingClockwise
                ? CirclePhase.CirclingAnticlockwise
                : CirclePhase.CirclingClockwise;

            destination = position - movementDirection;
            actor.MovementManager.StartPathfinding(destination);
        }
    }
}
