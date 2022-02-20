using UnityEngine;
using UnityEngine.AI;

public class PositionFinder
{
    private const int PositionsToSample = 4;

    private readonly Enemy targeter;
    private readonly Actor target;
    private readonly float minDistance;
    private readonly float maxDistance;

    public PositionFinder(Enemy targeter, Actor target, float minDistance, float maxDistance)
    {
        this.targeter = targeter;
        this.target = target;
        this.minDistance = minDistance;
        this.maxDistance = maxDistance;
    }

    /// Samples random positions to find one that is:
    ///  - A good distance from the target
    ///  - As far from the edge of the navmesh AND the targeter's current position as possible
    public Vector3 GetRandomPositionAroundTarget()
    {
        Vector3[] potentialPositions = GetPotentialPositions();

        Vector3 bestPosition = potentialPositions[0];
        float bestDistanceProduct = 0;

        // Choose the position with the greatest (distanceFromTargeter * distanceFromEdge)
        foreach (Vector3 position in potentialPositions)
        {
            if (!NavMesh.FindClosestEdge(position, out NavMeshHit navMeshHit, NavMesh.AllAreas)) continue;

            float distanceFromTargeter = Vector3.Distance(position, targeter.transform.position);
            float distanceFromEdge = Vector3.Distance(position, navMeshHit.position);

            float distanceProduct = distanceFromTargeter * distanceFromEdge;

            if (distanceProduct <= bestDistanceProduct) continue;

            bestPosition = position;
            bestDistanceProduct = distanceProduct;
        }

        return bestPosition;
    }

    /// Gets <see cref="PositionsToSample"/> random points on the navmesh that have distance from target between minDistance and maxDistance
    private Vector3[] GetPotentialPositions()
    {
        Vector3[] potentialPositions = new Vector3[PositionsToSample];

        int positionsFound = 0;
        while (positionsFound != PositionsToSample)
        {
            float distance = Random.Range(minDistance, maxDistance);
            Vector2 direction2D = Random.insideUnitCircle.normalized;
            Vector3 direction = new Vector3(direction2D.x, 0f, direction2D.y);
            Vector3 newPosition = target.transform.position + distance * direction;
            if (targeter.MovementManager.CanReach(newPosition))
            {
                potentialPositions[positionsFound] = newPosition;
                positionsFound++;
            }
        }

        return potentialPositions;
    }
}
