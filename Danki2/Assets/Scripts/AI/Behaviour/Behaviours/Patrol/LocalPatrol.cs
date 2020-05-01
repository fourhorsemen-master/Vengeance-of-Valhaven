using UnityEngine;
using UnityEngine.AI;

[Behaviour("Random patrol", new string[] { "Max destination distance", "Repath frequency" }, new AIAction[] { AIAction.Patrol })]
public class LocalPatrol : Behaviour
{
    private float maxDestinationDistance;
    private float repathFrequency;

    public override void Initialize()
    {
        maxDestinationDistance = Args[0];
        repathFrequency = Args[1];
    }

    public override void Behave(Actor actor)
    {
        // Set new destination randomly on avergae once per second (maths not guaranteed).
        if (Random.Range(0f, 1f) > Time.deltaTime * repathFrequency) return;

        Vector2 randomOffset = Random.insideUnitCircle * maxDestinationDistance;
        Vector3 randomDestination = actor.transform.position + new Vector3(randomOffset.x, randomOffset.y);

        if (NavMesh.SamplePosition(randomDestination, out NavMeshHit hit, 1f, NavMesh.AllAreas))
        {
            actor.MovementManager.StartPathfinding(hit.position);
        }
    }
}
