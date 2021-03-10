using UnityEngine;

/// <summary>
/// Maintains a Target Actor and a TargetLocation Vector3.
/// </summary>
public class PlayerTargetFinder
{
    private readonly Player player;
    private Subscription targetDeathSubscription;

    public Vector3 FloorTargetPosition { get; private set; } = Vector3.zero;
    public Vector3 OffsetTargetPosition { get; private set; } = Vector3.zero;

    public Enemy Target { get; private set; } = null;

    public PlayerTargetFinder(Player player, Subject updateSubject)
    {
        this.player = player;
        updateSubject.Subscribe(UpdateTarget);
    }

    private void UpdateTarget()
    {
        if (TryHitActor()) return;
        if (TryHitNavmesh()) return;
        HitPlane();
    }

    private bool TryHitActor()
    {
        bool mouseHitActor = MouseGamePositionFinder.Instance.TryGetCollider(
            out Collider collider,
            out _,
            LayerUtils.GetLayerMask(new[] { Layer.Actors })
        );

        if (!mouseHitActor || !RoomManager.Instance.TryGetActor(collider.gameObject, out _))
        {
            RemoveTarget();
            return false;
        }

        if (collider.CompareTag(Tag.Enemy))
        {
            Enemy enemy = collider.gameObject.GetComponent<Enemy>();
            SetTarget(enemy);
            return true;
        }

        RemoveTarget();

        if (collider.CompareTag(Tag.Player))
        {
            HitPlane();
            return true;
        }

        return false;
    }

    private void SetTarget(Enemy enemy)
    {
        SetTargetPositions(enemy.transform.position, enemy.Centre);
        
        if (enemy == Target) return;

        RemoveTarget();

        enemy.PlayerTargeted.Next(true);
        targetDeathSubscription = enemy.DeathSubject.Subscribe(RemoveTarget);
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

    private bool TryHitNavmesh()
    {
        bool mouseHitNavmesh = MouseGamePositionFinder.Instance.TryGetNavMeshPositions(
            out Vector3 floorPosition,
            out Vector3 offsetPosition
        );

        if (!mouseHitNavmesh) return false;

        SetTargetPositions(floorPosition, offsetPosition);
        return true;
    }

    private void HitPlane()
    {
        MouseGamePositionFinder.Instance.GetPlanePositions(
            player.transform.position.y,
            out Vector3 floorPosition,
            out Vector3 offsetPosition
        );

        SetTargetPositions(floorPosition, offsetPosition);
    }

    private void SetTargetPositions(Vector3 floorPosition, Vector3 offsetPosition)
    {
        FloorTargetPosition = floorPosition;
        OffsetTargetPosition = offsetPosition;
        player.ChannelService.FloorTargetPosition = floorPosition;
        player.ChannelService.OffsetTargetPosition = offsetPosition;
    }
}
