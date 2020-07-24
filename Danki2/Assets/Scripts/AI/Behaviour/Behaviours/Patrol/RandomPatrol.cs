using UnityEngine;

/// <summary>
/// With random patrol, we periodically path to a random location within maxDestinationDistance of the actor.
/// We repath roughly once every repathInterval seconds.
/// </summary>
[Behaviour("Random patrol", new string[] { "Max destination distance", "Repath interval", "Attention Distance" }, new AIAction[] { AIAction.Patrol })]
public class RandomPatrol : Behaviour
{
    private float maxDestinationDistance;
    private float repathInterval;
    private float attentionDistance;

    private bool repathedRecently = false;

    public override void DeserializeArgs()
    {
        maxDestinationDistance = Args[0];
        repathInterval = Args[1];
        attentionDistance = Args[2];
    }

    public override void Behave(Actor actor)
    {
        Wolf wolf = (Wolf) actor;

        if (TryWatchPlayer(wolf)) return;

        Patrol(wolf);
    }

    private bool TryWatchPlayer(Wolf wolf)
    {
        Player player = RoomManager.Instance.Player;

        float distanceToPlayer = Vector3.Distance(
            player.transform.position,
            wolf.transform.position
        );

        if (distanceToPlayer > attentionDistance)
        {
            wolf.LoseAttention();
            return false;
        }

        wolf.GetAttention(player);
        return true;
    }

    private void Patrol(Wolf wolf)
    {
        if (repathedRecently) return;

        Vector2 randomOffset = Random.insideUnitCircle * maxDestinationDistance;
        if (randomOffset.magnitude < 1f) return;

        Vector3 randomDestination = wolf.transform.position + new Vector3(randomOffset.x, 0f, randomOffset.y);
        if (wolf.MovementManager.TryStartPathfinding(randomDestination, 0.1f))
        {
            repathedRecently = true;

            wolf.WaitAndAct(
                repathInterval * Random.Range(0.5f, 2f),
                () => repathedRecently = false
            );
        }
    }
}
