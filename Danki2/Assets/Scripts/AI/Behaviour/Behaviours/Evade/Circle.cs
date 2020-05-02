using System;
using UnityEngine;
using UnityEngine.AI;

[Behaviour("Circle target", new string[] { "Max circle distance", "Min circle distance" }, new AIAction[] { AIAction.Evade })]
public class Circle : Behaviour
{
    private const float destinationTolerance = 0.5f;

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
            Debug.Log("Min circle distance is greater than max circle distance.");
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
            favouredCircleDistance = UnityEngine.Random.Range(minCircleDistance, maxCircleDistance);
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

            if (TryMove(actor, awayFromTarget))
            {
                phase = CirclePhase.MovingOut;
                actor.MovementManager.StartPathfinding(awayFromTarget);
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
            : clockwiseDirection * -1;

        Vector3 destination = position + movementDirection;

        if (!TryMove(actor, destination))
        {
            phase = phase == CirclePhase.CirclingClockwise
                ? CirclePhase.CirclingAnticlockwise
                : CirclePhase.CirclingClockwise;

            destination = position - movementDirection;
        }

        actor.MovementManager.StartPathfinding(destination);
    }

    private bool TryMove(Actor actor, Vector3 destination)
    {
        bool canMove = NavMesh.SamplePosition(destination, out NavMeshHit hit, destinationTolerance, NavMesh.AllAreas);

        if (canMove)
        {
            actor.MovementManager.StartPathfinding(hit.position);
        }

        return canMove;
    }
}
