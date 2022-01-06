using System.Linq;

public class EntAdvance : IStateMachineComponent
{
    private readonly Ent ent;
    private readonly Player player;
    private readonly float minDistance;
    private readonly float minRangedDistance;
    private readonly float maxRangedDistance;

    /// <summary>
    /// Advances toward the enemy if it is the closest enemy to the player, else moves away.
    /// </summary>
    /// <param name="ent"></param>
    /// <param name="player"></param>
    /// <param name="minDistance">The ent won't move closer to the player than this</param>
    /// <param name="maxDistance">The ent won't move away from the player if </param>
    public EntAdvance(
        Ent ent,
        Player player,
        float minDistance,
        float minRangedDistance,
        float maxRangedDistance)
    {
        this.ent = ent;
        this.player = player;
        this.minDistance = minDistance;
        this.minRangedDistance = minRangedDistance;
        this.maxRangedDistance = maxRangedDistance;
    }

    public void Enter() { }

    public void Exit()
    {
        ent.MovementManager.StopPathfinding();
        ent.MovementManager.ClearWatch();
    }

    public void Update()
    {
        float distanceFromPlayer = ent.transform.DistanceFromPlayer();

        float minDistanceFromPlayer = ActorCache.Instance.Cache
            .Where(x => !x.Actor.IsPlayer)
            .Min(x => x.Actor.transform.DistanceFromPlayer());

        if (distanceFromPlayer > minDistanceFromPlayer && distanceFromPlayer < maxRangedDistance) MoveAway(distanceFromPlayer);
        else MoveToward(distanceFromPlayer);
    }

    private void MoveAway(float distanceFromPlayer)
    {
        if (distanceFromPlayer < minRangedDistance)
        {
            ent.MovementManager.ClearWatch();
            ent.MovementManager.StartPathfinding(2 * ent.transform.position - player.transform.position);
        }
        else Stop();
    }

    private void MoveToward(float distanceFromPlayer)
    {
        if (distanceFromPlayer > minDistance)
        {
            ent.MovementManager.ClearWatch();
            ent.MovementManager.StartPathfinding(player.transform.position);
        }
        else Stop();
    }

    private void Stop()
    {
        ent.MovementManager.StopPathfinding();
        ent.MovementManager.Watch(player.transform);
    }
}
