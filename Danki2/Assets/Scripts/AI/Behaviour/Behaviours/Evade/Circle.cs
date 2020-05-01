using UnityEngine;
using UnityEngine.AI;

[Behaviour("Circle target", new string[] { "Max circle distance", "Min circle distance" }, new AIAction[] { AIAction.Evade })]
public class Circle : Behaviour
{
    private float maxCircleDistance;
    private float minCircleDistance;
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

        Vector3 position = actor.transform.position;
        Vector3 target = actor.Target.transform.position;

        Vector3 destination = position;

        float distanceToTarget = Vector3.Distance(position, target);

        if (
            distanceToTarget > maxCircleDistance
            || distanceToTarget > (minCircleDistance + maxCircleDistance) * 0.5f && phase == CirclePhase.MovingIn
        )
        {
            phase = CirclePhase.MovingIn;
            destination = target;
        }
        else if (
            distanceToTarget < minCircleDistance
            || distanceToTarget < (minCircleDistance + maxCircleDistance) * 0.5f && phase == CirclePhase.MovingOut
        )
        {
            phase = CirclePhase.MovingOut;
            destination = position + (position - target).normalized;
        }
        else
        {
            if (phase != CirclePhase.CirclingAnticlockwise && phase != CirclePhase.CirclingClockwise)
            {
                phase = CirclePhase.CirclingAnticlockwise;
            }

            Vector3 antiClockwiseDirection = Vector3.Cross(Vector3.up, target - position).normalized;
            Vector3 movementDirection = phase == CirclePhase.CirclingAnticlockwise
                ? antiClockwiseDirection
                : antiClockwiseDirection * -1;

            if (NavMesh.SamplePosition(position + movementDirection, out NavMeshHit hit, 1f, NavMesh.AllAreas))
            {
                destination = hit.position;
            }
            else
            {
                phase = phase == CirclePhase.CirclingAnticlockwise
                    ? CirclePhase.CirclingClockwise
                    : CirclePhase.CirclingAnticlockwise;
            }
        }

        actor.MovementManager.StartPathfinding(destination);
    }
}
