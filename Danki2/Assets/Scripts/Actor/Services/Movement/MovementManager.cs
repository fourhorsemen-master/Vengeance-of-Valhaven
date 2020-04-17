using UnityEngine;
using UnityEngine.AI;

public class MovementManager
{
    private readonly Actor actor;
    private readonly NavMeshAgent navMeshAgent;

    private const float RotationSmoothing = 0.05f;

    private Transform watchTarget = null;
    private bool watching = false;

    private Vector3 movementLockDirection;
    private float movementLockSpeed;

    private float remainingStatusDuration = 0f;

    private StateManager<MovementStatus> movementStatusManager;
    private MovementStatus MovementStatus => this.movementStatusManager.CurrentState;

    public MovementManager(Actor actor, Subject updateSubject, NavMeshAgent navMeshAgent)
    {
        this.actor = actor;
        this.navMeshAgent = navMeshAgent;
        updateSubject.Subscribe(UpdateMovement);

        this.movementStatusManager = new StateManager<MovementStatus>(MovementStatus.AbleToMove, ClearMovementStatus)
            .WithTransition(MovementStatus.AbleToMove, MovementStatus.Stunned)
            .WithTransition(MovementStatus.AbleToMove, MovementStatus.Rooted)
            .WithTransition(MovementStatus.AbleToMove, MovementStatus.MovementLocked)
            .WithTransition(MovementStatus.Stunned, MovementStatus.AbleToMove)
            .WithTransition(MovementStatus.Rooted, MovementStatus.AbleToMove)
            .WithTransition(MovementStatus.Rooted, MovementStatus.Stunned)
            .WithTransition(MovementStatus.MovementLocked, MovementStatus.AbleToMove)
            .WithTransition(MovementStatus.MovementLocked, MovementStatus.Stunned)
            .WithTransition(MovementStatus.MovementLocked, MovementStatus.Rooted);
    }

    /// <summary>
    /// Path towards the destination using navmesh pathfinding unless rooted, stunned or movement locked.
    /// </summary>
    /// <param name="destination"></param>
    public void StartPathfinding(Vector3 destination)
    {
        if (MovementStatus != MovementStatus.AbleToMove) return;

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
        if (MovementStatus != MovementStatus.AbleToMove) return;

        if (direction == Vector3.zero) return;

        ClearWatch();

        this.navMeshAgent.Move(direction.normalized * (Time.deltaTime * actor.GetStat(Stat.Speed)));

        RotateTowards(direction);
    }

    /// <summary>
    /// Lerp rotate the actor to face the target until we begin movement.
    /// </summary>
    /// <param name="watchTarget"></param>
    public void Watch(Transform watchTarget)
    {
        this.watchTarget = watchTarget;
        this.watching = true;
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
        this.navMeshAgent.ResetPath();
    }

    /// <summary>
    /// Lock the position and rotation for the given duration.
    /// </summary>
    /// <param name="duration"></param>
    public void Stun(float duration)
    {
        if (
            !this.movementStatusManager.CanTransition(MovementStatus.Stunned)
            || (MovementStatus == MovementStatus.Stunned && duration < this.remainingStatusDuration)
        ) return;

        this.movementStatusManager.Transition(MovementStatus.Stunned);
        StopPathfinding();
        this.navMeshAgent.isStopped = true;
        this.remainingStatusDuration = duration;
    }

    /// <summary>
    /// Lock the position for the given duration (can still rotate).
    /// </summary>
    /// <param name="duration"></param>
    public void Root(float duration)
    {
        if (
            !this.movementStatusManager.CanTransition(MovementStatus.Rooted) 
            || (MovementStatus == MovementStatus.Rooted && duration < this.remainingStatusDuration)
        ) return;

        this.movementStatusManager.Transition(MovementStatus.Rooted);
        StopPathfinding();
        this.navMeshAgent.isStopped = true;
        this.remainingStatusDuration = duration;
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
        if (!this.movementStatusManager.CanTransition(MovementStatus.MovementLocked)) return;

        this.movementStatusManager.Transition(MovementStatus.MovementLocked);
        this.remainingStatusDuration = duration;
        this.movementLockSpeed = speed;
        this.movementLockDirection = direction.normalized;

        this.actor.transform.rotation = Quaternion.LookRotation(rotation);
    }

    private void UpdateMovement()
    {
        this.navMeshAgent.speed = this.actor.GetStat(Stat.Speed);

        if (
            this.watching 
            && MovementStatus != MovementStatus.Stunned 
            && MovementStatus != MovementStatus.MovementLocked
        )
        {
            RotateTowards(watchTarget.position - this.actor.transform.position);
        }

        if (MovementStatus == MovementStatus.MovementLocked)
        {
            this.navMeshAgent.Move(movementLockDirection * (Time.deltaTime * movementLockSpeed));
        }

        if (MovementStatus != MovementStatus.AbleToMove)
        {
            this.remainingStatusDuration -= Time.deltaTime;
            if (this.remainingStatusDuration < 0f)
            {
                this.movementStatusManager.Transition(MovementStatus.AbleToMove);
            }
        }
    }

    private void ClearMovementStatus()
    {
        this.remainingStatusDuration = 0f;
        this.navMeshAgent.isStopped = false;
        this.movementLockSpeed = 0f;
        this.movementLockDirection = Vector3.zero;
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