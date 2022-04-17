﻿using UnityEngine;
using UnityEngine.AI;

public class EnemyMovementManager : MovementManager
{
    private readonly Enemy enemy;

    private const float DestinationTolerance = 0.5f;

    private bool movementLocked = false;
    private float movementLockSpeed;
    private Vector3 movementLockDirection;
    private Coroutine endMoveLockCoroutine;

    protected override float RotationSmoothing => rotationSmoothingOverride ?? enemy.RotationSmoothing;

    public bool CanMove => !enemy.Dead && !movementPaused && !movementLocked;

    public EnemyMovementManager(Enemy enemy, Subject updateSubject, NavMeshAgent navMeshAgent)
        : base(enemy, navMeshAgent)
    {
        this.enemy = enemy;
        this.enemy.DeathSubject.Subscribe(_ => StopPathfinding());

        updateSubject.Subscribe(UpdateMovement);

        navMeshAgent.updateRotation = false;
    }

    /// <summary>
    /// Path towards the destination using navmesh pathfinding unless rooted, stunned or movement locked.
    /// </summary>
    public void StartPathfinding(Vector3 destination)
    {
        if (!CanMove) return;

        navMeshAgent.isStopped = false;

        if (navMeshAgent.destination == destination) return;

        ClearWatch();

        navMeshAgent.SetDestination(destination);
    }

    public bool CanPathToDestination(Vector3 destination, float tolerance = DestinationTolerance)
    {
        return NavMesh.SamplePosition(destination, out NavMeshHit hit, tolerance, NavMesh.AllAreas);
    }

    /// <summary>
    /// Move along the navmesh in the given direction unless rooted, stunned or movement locked.
    /// </summary>
    /// <param name="speed"> Defaults to the actors speed stat. </param>
    public void Move(Vector3 direction, float? speed = null)
    {
        if (enemy.Dead) return;

        StopPathfinding();

        if (direction == Vector3.zero) return;

        ClearWatch();

        if (speed == null) speed = GetMoveSpeed();

        if(CanStrafeTarget(direction))
		{
            navMeshAgent.Move(direction.normalized * (Time.deltaTime * speed.Value));
		}
    }

    /// <summary>
    /// Lerp rotate the actor to face the target until we begin movement.
    /// </summary>
    public void Watch(Transform target, float? rotationSmoothingOverride = null)
    {
        watchTarget = target;
        watching = true;
        this.rotationSmoothingOverride = rotationSmoothingOverride;
    }

    public void ClearWatch()
    {
        watchTarget = null;
        watching = false;
        rotationSmoothingOverride = null;
    }

    public void LockMovement(float duration, float speed, Vector3 direction, Vector3 rotation)
    {
        StopPathfinding();
        ClearWatch();
        movementLocked = true;
        movementLockSpeed = speed;
        movementLockDirection = direction.normalized;

        if (rotation != Vector3.zero) Look(rotation);

        if (endMoveLockCoroutine != null)
        {
            enemy.StopCoroutine(endMoveLockCoroutine);
        }

        endMoveLockCoroutine = enemy.WaitAndAct(duration, () => {
            movementLocked = false;
        });
    }

    public void StopPathfinding()
    {
        navMeshAgent.ResetPath();
        navMeshAgent.isStopped = true;
    }

    public bool CanReach(Vector3 position)
    {
        NavMeshPath navMeshPath = new NavMeshPath();
        return navMeshAgent.CalculatePath(position, navMeshPath) && navMeshPath.status == NavMeshPathStatus.PathComplete;
    }

    private void UpdateMovement()
    {
        if (enemy.Dead) return;

        if (movementPaused && !navMeshAgent.isStopped) StopPathfinding();

        navMeshAgent.speed = GetMoveSpeed();

        RotateTowards(watchTarget.position - enemy.transform.position);

        if (movementLocked)
        {
            if(CanStrafeTarget(watchTarget.position))
			{
                navMeshAgent.Move(movementLockDirection * (Time.deltaTime * movementLockSpeed));
			}
        }
    }
}
