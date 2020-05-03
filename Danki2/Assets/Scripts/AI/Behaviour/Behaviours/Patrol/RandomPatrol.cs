using UnityEngine;
using UnityEngine.AI;

[Behaviour("Random patrol", new string[] { "Max destination distance", "Repath interval" }, new AIAction[] { AIAction.Patrol })]
public class RandomPatrol : Behaviour
{
    private float maxDestinationDistance;
    private float repathInterval;

    private bool repathedRecently = false;

    public override void Initialize()
    {
        maxDestinationDistance = Args[0];
        repathInterval = Args[1];
    }

    public override void Behave(Actor actor)
    {
        if (repathedRecently) return;

        // Set new destination randomly on avergae once per second (maths not guaranteed).
        if (Random.Range(0f, 1f) > Time.deltaTime * repathInterval) return;

        Vector2 randomOffset = Random.insideUnitCircle * maxDestinationDistance;
        if (randomOffset.magnitude < 1f) return;

        Vector3 randomDestination = actor.transform.position + new Vector3(randomOffset.x, 0f, randomOffset.y);
        if (NavMesh.SamplePosition(randomDestination, out NavMeshHit hit, 0.1f, NavMesh.AllAreas))
        {
            actor.MovementManager.StartPathfinding(hit.position);
            repathedRecently = true;

            actor.WaitAndAct(
                repathInterval * Random.Range(0.5f, 2f),
                () => repathedRecently = false
            );
        }
    }
}
