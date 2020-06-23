using UnityEngine;
using UnityEngine.AI;

public class MovementManager
{
    private readonly Actor actor;
    private readonly NavMeshAgent navMeshAgent;

    private const float DestinationTolerance = 0.5f;
    private const float RotationSmoothing = 0.15f;

    private Transform watchTarget = null;
    private bool watching = false;

    private Vector3 movementLockDirection;
    private float movementLockSpeed;

    private MovementStatusManager movementStatusManager;

    public bool Stunned => movementStatusManager.Stunned;
    public bool Rooted => movementStatusManager.Rooted;
    public bool MovementLocked => movementStatusManager.MovementLocked;
    public bool CanMove => !movementStatusManager.Stunned && !movementStatusManager.Rooted && !movementStatusManager.MovementLocked;

    public bool IsMoving { get; private set; } = false;
    private bool movedThisFrame = false;

    public MovementManager(Actor actor, Subject updateSubject, NavMeshAgent navMeshAgent)
    {
        this.actor = actor;
        this.navMeshAgent = navMeshAgent;
        updateSubject.Subscribe(UpdateMovement);
        movementStatusManager = new MovementStatusManager(updateSubject);
    }

    /// <summary>
    /// Path towards the destination using navmesh pathfinding unless rooted, stunned or movement locked.
    /// </summary>
    /// <param name="destination"></param>
    public void StartPathfinding(Vector3 destination)
    {
        if (!CanMove) return;

        navMeshAgent.isStopped = false;

        if (navMeshAgent.destination == destination) return;

        ClearWatch();

        navMeshAgent.SetDestination(destination);
    }

    /// <summary>
    /// Starts pathing to navmesh point closest to given destination IF those points are within <see cref="DefaultBar"/> of eachother.
    /// </summary>
    /// <param name="destination"></param>
    /// <returns>True if started pathfinding</returns>
    public bool TryStartPathfinding(Vector3 destination, float tolerance = DestinationTolerance)
    {
        if (!CanMove) return false;

        bool canPathToDestination = NavMesh.SamplePosition(destination, out NavMeshHit hit, tolerance, NavMesh.AllAreas);

        if (canPathToDestination)
        {
            StartPathfinding(hit.position);
        }

        return canPathToDestination;
    }

    /// <summary>
    /// Move along the navmesh in the given direction unless rooted, stunned or movement locked.
    /// </summary>
    /// <param name="direction"></param>
    public void Move(Vector3 direction)
    {
        StopPathfinding();

        if (!CanMove) return;

        if (direction == Vector3.zero) return;

        ClearWatch();

        navMeshAgent.Move(direction.normalized * (Time.deltaTime * actor.GetStat(Stat.Speed)));
        movedThisFrame = true;

        RotateTowards(direction);
    }

    /// <summary>
    /// Lerp rotate the actor to face the target until we begin movement.
    /// </summary>
    /// <param name="watchTarget"></param>
    public void Watch(Transform target)
    {
        watchTarget = target;
        watching = true;
    }

    /// <summary>
    /// Snap rotate the actor to face the position.
    /// </summary>
    /// <param name="position"></param>
    public void LookAt(Vector3 position)
    {
        actor.transform.rotation = Quaternion.LookRotation(position - actor.transform.position);
    }

    public void StopPathfinding()
    {
        navMeshAgent.ResetPath();
    }

    /// <summary>
    /// Lock the position and rotation for the given duration.
    /// </summary>
    /// <param name="duration"></param>
    public void Stun(float duration)
    {
        movementStatusManager.Stun(duration);
        StopPathfinding();
        navMeshAgent.isStopped = true;
    }

    /// <summary>
    /// Lock the position for the given duration (can still rotate).
    /// </summary>
    /// <param name="duration"></param>
    public void Root(float duration)
    {
        movementStatusManager.Root(duration);
        StopPathfinding();
        navMeshAgent.isStopped = true;
    }

    public void KnockBack(float duration, float speed, Vector3 direction, Vector3 rotation)
    {
        LockMovement(false, duration, speed, direction, rotation);
    }

    public void Dash(float duration, float speed, Vector3 direction, Vector3 rotation)
    {
        LockMovement(true, duration, speed, direction, rotation);
    }

    /// <summary>
    /// Lock movement velocity for a given duration with a fixed rotation.
    /// </summary>
    /// <param name="selfLock"></param>
    /// <param name="duration"></param>
    /// <param name="speed"></param>
    /// <param name="direction"></param>
    /// <param name="rotation">The rotation to maintain for the duration.</param>
    private void LockMovement(bool selfLock, float duration, float speed, Vector3 direction, Vector3 rotation)
    {
        if (!movementStatusManager.TryLockMovement(selfLock, duration)) return;

        StopPathfinding();
        movementLockSpeed = speed;
        movementLockDirection = direction.normalized;

        if (rotation != Vector3.zero)
        {
            actor.transform.rotation = Quaternion.LookRotation(rotation);
        }
    }

    private void UpdateMovement()
    {
        IsMoving = movedThisFrame || (navMeshAgent.hasPath && navMeshAgent.velocity.magnitude > 0f);
        movedThisFrame = false;

        navMeshAgent.speed = actor.GetStat(Stat.Speed);

        if (
            watching 
            && !movementStatusManager.Stunned
            && !movementStatusManager.MovementLocked
        )
        {
            RotateTowards(watchTarget.position - actor.transform.position);
        }

        if (movementStatusManager.MovementLocked)
        {
            navMeshAgent.Move(movementLockDirection * (Time.deltaTime * movementLockSpeed));
        }
    }

    private void ClearWatch()
    {
        watchTarget = null;
        watching = false;
    }

    private void RotateTowards(Vector3 direction)
    {
        if (!direction.Equals(Vector3.zero))
        {
            Quaternion desiredRotation = Quaternion.LookRotation(direction);
            actor.transform.rotation = Quaternion.Lerp(actor.transform.rotation, desiredRotation, RotationSmoothing);
        }
    }
}