using UnityEngine;

/// <summary>
/// Maintains a Target Actor and a TargetLocation Vector3.
/// </summary>
public class PlayerTargetFinder
{
    private readonly Player player;
    private Subscription targetDeathSubscription;

    public Vector3 TargetPosition { get; private set; }

    public Enemy Target { get; private set; }

    public PlayerTargetFinder(Player player, Subject updateSubject)
    {
        this.player = player;
        updateSubject.Subscribe(UpdateTarget);
    }

    private void UpdateTarget()
    {
        // First, we raycast for an actor (ie. by ignoring other layers)
        bool mouseHitCollider = MouseGamePositionFinder.Instance.TryGetMouseGamePosition(
            out Vector3 mousePosition,
            out Collider collider,
            Layers.GetLayerMask(new[] { Layers.Actors })
        );

        // Then, if we don't hit any actors, we raycast for any collider
        if (!mouseHitCollider)
        {
            mouseHitCollider = MouseGamePositionFinder.Instance.TryGetMouseGamePosition(out mousePosition, out collider);
        }

        // Then, if no colliders are hit, we use the mouse position on a horizontal plane at the players height.
        if (!mouseHitCollider)
        {
            mousePosition = MouseGamePositionFinder.Instance.GetMousePlanePosition(player.transform.position.y, true);
        }

        SetTargetPosition(mousePosition);

        if (collider != null && collider.gameObject.CompareTag(Tags.Enemy))
        {
            Enemy enemy = collider.gameObject.GetComponent<Enemy>();
            if (!enemy.Dead) SetTarget(enemy);
        }
        else
        {
            RemoveTarget();
        }
    }

    private void SetTargetPosition(Vector3 mousePosition)
    {
        TargetPosition = mousePosition;
        player.ChannelService.TargetPosition = TargetPosition;
    }

    private void SetTarget(Enemy enemy)
    {
        if (enemy == Target) return;

        RemoveTarget();

        enemy.PlayerTargeted.Next(true);
        targetDeathSubscription = enemy.DeathSubject.Subscribe(() => RemoveTarget());
        Target = enemy;
        player.ChannelService.Target = enemy;
    }

    private void RemoveTarget()
    {
        if (Target == null) return;

        Target.PlayerTargeted.Next(false);
        targetDeathSubscription.Unsubscribe();
        Target = null;
        player.ChannelService.Target = null;
    }
}
