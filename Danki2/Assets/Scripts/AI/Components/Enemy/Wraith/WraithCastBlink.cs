using UnityEngine;
using UnityEngine.AI;

public class WraithCastBlink : IStateMachineComponent
{
    private const int PositionsToSample = 4;
    
    private readonly Wraith wraith;
    private readonly Actor actorToAvoid;
    private readonly float minDistance;
    private readonly float maxDistance;

    public WraithCastBlink(Wraith wraith, Actor actorToAvoid, float minDistance, float maxDistance)
    {
        this.wraith = wraith;
        this.actorToAvoid = actorToAvoid;
        this.minDistance = minDistance;
        this.maxDistance = maxDistance;
    }

    public void Enter() => wraith.Blink(GetNewPosition());
    public void Exit() {}
    public void Update() {}

    /// Samples random positions to find one that is:
    ///  - A good distance from the player
    ///  - As far from the edge of the navmesh AND the wraith's current position as possible
    private Vector3 GetNewPosition()
    {
        Vector3[] potentialPositions = GetPotentialPositions();

        Vector3 bestPosition = potentialPositions[0];
        float bestDistanceProduct = 0;

        // Choose the position with the greatest (distanceFromWraith * distanceFromEdge)
        foreach (Vector3 position in potentialPositions)
        {
            if (!NavMesh.FindClosestEdge(position, out NavMeshHit navMeshHit, NavMesh.AllAreas)) continue;

            float distanceFromWraith = Vector3.Distance(position, wraith.transform.position);
            float distanceFromEdge = Vector3.Distance(position, navMeshHit.position);

            float distanceProduct = distanceFromWraith * distanceFromEdge;

            if (distanceProduct <= bestDistanceProduct) continue;

            bestPosition = position;
            bestDistanceProduct = distanceProduct;
        }

        return bestPosition;
    }

    /// Gets <see cref="PositionsToSample"/> random points on the navmesh that have distance from player between minDistance and maxDistance
    private Vector3[] GetPotentialPositions()
    {
        Vector3[] potentialPositions = new Vector3[PositionsToSample];

        int positionsFound = 0;
        while (positionsFound != PositionsToSample)
        {
            float distance = Random.Range(minDistance, maxDistance);
            Vector2 direction2D = Random.insideUnitCircle.normalized;
            Vector3 direction = new Vector3(direction2D.x, 0f, direction2D.y);
            Vector3 newPosition = actorToAvoid.transform.position + distance * direction;
            if (wraith.MovementManager.CanReach(newPosition))
            {
                potentialPositions[positionsFound] = newPosition;
                positionsFound++;
            }
        }

        return potentialPositions;
    }
}
