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

    public void SetDestination(Vector3 destination)
    {
        if (this.stunned || this.rooted || this.movementLocked) return;

        if (this.navMeshAgent.destination == destination) return;

        ClearWatch();

        this.navMeshAgent.SetDestination(destination);
    }

    public void Move(Vector3 direction)
    {
        if (this.stunned || this.rooted || this.movementLocked) return;

        if (direction == Vector3.zero) return;

        ClearWatch();

        this.navMeshAgent.Move(direction.normalized * Time.deltaTime * actor.GetStat(Stat.Speed));

        RotateTorwards(direction);
    }

    public void Watch(Transform watchTarget)
    {
        this.watchTarget = watchTarget;
        this.watching = true;
    }

    public void ClearDestination()
    {
        this.navMeshAgent.ResetPath();
    }

    public void Stun(float duration)
    {
        if (this.stunned && duration < this.remainingStunDuration) return;

        ClearRoot();
        ClearMovementLock();

        ClearDestination();
        this.stunned = true;
        this.navMeshAgent.isStopped = true;
        this.remainingStunDuration = duration;
    }

    public void Root(float duration)
    {
        if (this.stunned || (this.rooted && duration < this.remainingRootDuration)) return;

        ClearMovementLock();

        ClearDestination();
        this.rooted = true;
        this.navMeshAgent.isStopped = true;
        this.remainingRootDuration = duration;
    }

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
            RotateTorwards(watchTarget.position - this.actor.transform.position);
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
            this.navMeshAgent.Move(movementLockDirection * Time.deltaTime * movementLockSpeed);

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

    private void RotateTorwards(Vector3 direction)
    {
        if (!direction.Equals(Vector3.zero))
        {
            Quaternion desiredRotation = Quaternion.LookRotation(direction);
            this.actor.transform.rotation = Quaternion.Lerp(actor.transform.rotation, desiredRotation, RotationSmoothing);
        }
    }
}