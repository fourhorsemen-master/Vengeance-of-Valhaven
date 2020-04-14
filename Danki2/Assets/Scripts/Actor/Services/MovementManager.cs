using UnityEngine;
using UnityEngine.AI;

public class MovementManager
{
    private readonly Actor actor;
    private readonly NavMeshAgent navMeshAgent;

    private const float RotationSmoothing = 0.05f;

    private Transform watchTarget = null;
    private bool watching = false;

    private bool stunned = false;
    private bool rooted = false;
    private bool movementLocked = false;
    private Vector3 movementLockDirection;
    private float movementLockSpeed;

    private float remainingStunDuration = 0f;
    private float remainingRootDuration = 0f;
    private float remainingMovementLockDuration = 0f;

    public MovementManager(Actor actor, Subject updateSubject, NavMeshAgent navMeshAgent)
    {
        this.actor = actor;
        this.navMeshAgent = navMeshAgent;
        updateSubject.Subscribe(UpdateMovement);
    }

    /// <summary>
    /// Path towards the destination using navmesh pathfinding unless rooted, stunned or movement locked.
    /// </summary>
    /// <param name="destination"></param>
    public void StartPathfinding(Vector3 destination)
    {
        if (this.stunned || this.rooted || this.movementLocked) return;

        if (this.navMeshAgent.destination == destination) return;

        ClearWatch();

        this.navMeshAgent.SetDestination(destination);
    }

    /// <summary>
    /// Move along the navmesh in the given direction unless rooted, stunned or movement locked.
    /// </summary>
    /// <param name="direction"></param>
    public void Move(Vector3 direction)
    {
        if (this.stunned || this.rooted || this.movementLocked) return;

        if (direction == Vector3.zero) return;

        ClearWatch();

        this.navMeshAgent.Move(direction.normalized * (Time.deltaTime * actor.GetStat(Stat.Speed)));

        RotateTowards(direction);
    }

    /// <summary>
    /// Rotate the actor to face the target until we begin movement.
    /// </summary>
    /// <param name="watchTarget"></param>
    public void Watch(Transform watchTarget)
    {
        this.watchTarget = watchTarget;
        this.watching = true;
    }

    public void StopPathfinding()
    {
        this.navMeshAgent.ResetPath();
    }

    /// <summary>
    /// Lock the position and rotation for the given duration.
    /// </summary>
    /// <param name="duration"></param>
    public void Stun(float duration)
    {
        if (this.stunned && duration < this.remainingStunDuration) return;

        ClearRoot();
        ClearMovementLock();

        StopPathfinding();
        this.stunned = true;
        this.navMeshAgent.isStopped = true;
        this.remainingStunDuration = duration;
    }

    /// <summary>
    /// Lock the position for the given duration (can still rotate).
    /// </summary>
    /// <param name="duration"></param>
    public void Root(float duration)
    {
        if (this.stunned || (this.rooted && duration < this.remainingRootDuration)) return;

        ClearMovementLock();

        StopPathfinding();
        this.rooted = true;
        this.navMeshAgent.isStopped = true;
        this.remainingRootDuration = duration;
    }

    /// <summary>
    /// Lock movement velocity for a given duration with a fixed rotation.
    /// </summary>
    /// <param name="duration"></param>
    /// <param name="speed"></param>
    /// <param name="direction"></param>
    /// <param name="rotation">The rotation to maintain for the duration.</param>
    public void LockMovement(float duration, float speed, Vector3 direction, Vector3 rotation)
    {
        if (this.stunned || this.rooted || this.movementLocked) return;

        this.movementLocked = true;
        this.remainingMovementLockDuration = duration;
        this.movementLockSpeed = speed;
        this.movementLockDirection = direction.normalized;

        this.actor.transform.rotation = Quaternion.LookRotation(rotation);
    }

    private void UpdateMovement()
    {
        this.navMeshAgent.speed = this.actor.GetStat(Stat.Speed);

        if (this.watching && !this.stunned && !this.movementLocked)
        {
            RotateTowards(watchTarget.position - this.actor.transform.position);
        }

        if (this.stunned)
        {
            this.remainingStunDuration -= Time.deltaTime;
            if (this.remainingStunDuration < 0f) ClearStun();
        }

        if (this.rooted)
        {
            this.remainingRootDuration -= Time.deltaTime;
            if (this.remainingRootDuration < 0f) ClearRoot();
        }

        if (this.movementLocked)
        {
            this.navMeshAgent.Move(movementLockDirection * (Time.deltaTime * movementLockSpeed));

            this.remainingMovementLockDuration -= Time.deltaTime;
            if (this.remainingMovementLockDuration < 0f) ClearMovementLock();
        }
    }

    private void ClearMovementLock()
    {
        this.movementLocked = false;
        this.remainingMovementLockDuration = 0f;
        this.movementLockSpeed = 0f;
        this.movementLockDirection = Vector3.zero;
    }

    private void ClearRoot()
    {
        this.rooted = false;
        this.remainingRootDuration = 0f;
        this.navMeshAgent.isStopped = false;
    }

    private void ClearStun()
    {
        this.stunned = false;
        this.remainingStunDuration = 0f;
        this.navMeshAgent.isStopped = false;
    }

    private void ClearWatch()
    {
        this.watchTarget = null;
        this.watching = false;
    }

    private void RotateTowards(Vector3 direction)
    {
        if (!direction.Equals(Vector3.zero))
        {
            Quaternion desiredRotation = Quaternion.LookRotation(direction);
            this.actor.transform.rotation = Quaternion.Lerp(actor.transform.rotation, desiredRotation, RotationSmoothing);
        }
    }
}