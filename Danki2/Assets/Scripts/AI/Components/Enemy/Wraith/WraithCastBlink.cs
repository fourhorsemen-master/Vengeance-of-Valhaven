using System.Linq;
using UnityEngine;

public class WraithCastBlink : IStateMachineComponent
{
    private const int PositionsToSample = 5;
    
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

    private Vector3 GetNewPosition()
    {
        return GetPotentialPositions()
            .OrderBy(p => Vector3.Distance(p, actorToAvoid.transform.position))
            .Last();
    }

    private Vector3[] GetPotentialPositions()
    {
        Vector3[] potentialPositions = new Vector3[PositionsToSample];

        int positionsFound = 0;
        while (positionsFound != PositionsToSample)
        {
            float distance = Random.Range(minDistance, maxDistance);
            Vector2 direction2D = Random.insideUnitCircle.normalized;
            Vector3 direction = new Vector3(direction2D.x, 0f, direction2D.y);
            Vector3 newPosition = wraith.transform.position + distance * direction;
            if (wraith.MovementManager.CanReach(newPosition))
            {
                potentialPositions[positionsFound] = newPosition;
                positionsFound++;
            }
        }

        return potentialPositions;
    }
}
