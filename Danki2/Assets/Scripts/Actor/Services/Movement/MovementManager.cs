﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MovementManager : IMovementStatusProvider
{
    private static readonly ISet<MovementLockType> MoveLockOverrideTypes = new HashSet<MovementLockType>
    {
        MovementLockType.Knockback,
        MovementLockType.Pull
    };

    private static readonly ISet<MovementLockType> MovementLockTypesAffectedByWeight = new HashSet<MovementLockType>
    {
        MovementLockType.Knockback,
        MovementLockType.Pull
    };

    private readonly Actor actor;
    private readonly NavMeshAgent navMeshAgent;

    private const float MovementSpeedMultiplier = 0.01f;

    private const float WalkSpeedMultiplier = 0.3f;
    private const float DestinationTolerance = 0.5f;

    private Transform watchTarget = null;
    private bool watching = false;
    private float? rotationSmoothingOverride = null;

    private Vector3 movementLockDirection;
    private float movementLockSpeed;

    private bool movementPaused = false;
    private Coroutine endPauseCoroutine = null;

    private MovementStatusManager movementStatusManager;

    public bool Stunned => movementStatusManager.Stunned;
    public bool Rooted => movementStatusManager.Rooted;
    public bool MovementLocked => movementStatusManager.MovementLocked;
    public bool CanMove => !actor.Dead
        && !movementStatusManager.Stunned
        && !movementStatusManager.Rooted
        && !movementStatusManager.MovementLocked;

    public MotionType MotionType { get; set; } = MotionType.Run;

    public bool IsMoving { get; private set; } = false;
    private bool movedThisFrame = false;

    public Subject MoveLockSubject { get; } = new Subject();

    private float RotationSmoothing => rotationSmoothingOverride ?? actor.RotationSmoothing;

    public MovementManager(Actor actor, Subject updateSubject, NavMeshAgent navMeshAgent)
    {
        this.actor = actor;
        actor.DeathSubject.Subscribe(StopPathfinding);

        this.navMeshAgent = navMeshAgent;

        updateSubject.Subscribe(UpdateMovement);
        movementStatusManager = new MovementStatusManager(updateSubject);
        movementStatusManager.RegisterProviders(this, actor.ChannelService, new StunHandler(actor));
    }

    public bool Stuns() => movementPaused;

    public bool Roots() => false;

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
    /// <param name="speed"> Defaults to the actors speed stat. </param>
    public void Move(Vector3 direction, float? speed = null)
    {
        if (actor.Dead) return;

        StopPathfinding();

        if (Stunned || MovementLocked) return;

        if (direction == Vector3.zero) return;

        ClearWatch();

        RotateTowards(direction);

        if (Rooted) return;

        if (speed == null) speed = GetMoveSpeed();

        navMeshAgent.Move(direction.normalized * (Time.deltaTime * speed.Value));
        movedThisFrame = true;
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

    /// <summary>
    /// Snap rotate the actor to face the position.
    /// </summary>
    /// <param name="position"></param>
    public void LookAt(Vector3 position)
    {
        Look(position - actor.transform.position);
    }

    public void StopPathfinding()
    {
        navMeshAgent.ResetPath();
    }

    /// <summary>
    /// Lock the position and rotation for the given duration.
    /// </summary>
    /// <param name="duration"></param>
    public void Pause(float duration)
    {
        movementPaused = true;

        if (endPauseCoroutine != null) actor.StopCoroutine(endPauseCoroutine);

        endPauseCoroutine = actor.WaitAndAct(duration, () => {
            movementPaused = false;
            endPauseCoroutine = null;
        });
    }

    public bool TryLockMovement(MovementLockType type, float duration, float speed, Vector3 direction, Vector3 rotation)
    {
        bool overrideLock = MoveLockOverrideTypes.Contains(type);

        if (MovementLockTypesAffectedByWeight.Contains(type)) speed /= actor.Weight;

        return TryLockMovement(overrideLock, duration, speed, direction, rotation);
    }

    public bool CanReach(Vector3 position)
    {
        NavMeshPath navMeshPath = new NavMeshPath();
        return navMeshAgent.CalculatePath(position, navMeshPath) && navMeshPath.status == NavMeshPathStatus.PathComplete;
    }

    /// <summary>
    /// Lock movement velocity for a given duration with a fixed rotation.
    /// </summary>
    /// <param name="overrideLock"></param>
    /// <param name="duration"></param>
    /// <param name="speed"></param>
    /// <param name="direction"></param>
    /// <param name="rotation">The rotation to maintain for the duration.</param>
    private bool TryLockMovement(bool overrideLock, float duration, float speed, Vector3 direction, Vector3 rotation)
    {
        if (!movementStatusManager.TryLockMovement(overrideLock, duration)) return false;

        if (overrideLock) actor.InterruptionManager.Interrupt(InterruptionType.Hard);

        StopPathfinding();
        movementLockSpeed = speed;
        movementLockDirection = direction.normalized;

        if (rotation != Vector3.zero) Look(rotation);

        MoveLockSubject.Next();
        return true;
    }

    private void Look(Vector3 rotation)
    {
        rotation.y = 0;
        actor.transform.rotation = Quaternion.LookRotation(rotation);
    }

    private void UpdateMovement()
    {
        if (actor.Dead) return;

        if (!CanMove && !navMeshAgent.isStopped)
        {
            StopPathfinding();
            navMeshAgent.isStopped = true;
        }

        IsMoving = movedThisFrame || (navMeshAgent.hasPath && navMeshAgent.velocity.magnitude > 0f);

        if (actor.AnimController)
        {
            float blendValue = movedThisFrame ? 1f : 0f;
            actor.AnimController.SetFloat("MoveSpeed_Blend", blendValue, 0.1f, Time.deltaTime);
        }

        movedThisFrame = false;

        navMeshAgent.speed = GetMoveSpeed();

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

    private void RotateTowards(Vector3 direction)
    {
        if (!direction.Equals(Vector3.zero))
        {
            Quaternion desiredRotation = Quaternion.LookRotation(direction);
            float lerpAmount = MathUtils.GetDeltaTimeLerpAmount(RotationSmoothing);
            actor.transform.rotation = Quaternion.Lerp(actor.transform.rotation, desiredRotation, lerpAmount);
        }
    }

    private float GetMoveSpeed()
    {
        float moveSpeed = actor.StatsManager.Get(Stat.Speed) * MovementSpeedMultiplier;

        return MotionType == MotionType.Run
            ? moveSpeed
            : moveSpeed * WalkSpeedMultiplier;
    }
}
