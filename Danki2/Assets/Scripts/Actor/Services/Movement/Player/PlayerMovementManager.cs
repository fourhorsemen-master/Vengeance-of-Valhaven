using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovementManager : MovementManager, IMovementStatusProvider
{
    private readonly Player player;

    private Vector3 movementLockDirection;
    private float movementLockSpeed;

    private readonly MovementStatusManager movementStatusManager;

    public bool Stunned => movementStatusManager.Stunned;
    public bool MovementLocked => movementStatusManager.MovementLocked;

    private bool isCasting = false;

    protected override float RotationSmoothing => player.RotationSmoothing;

    public bool CanMove => !player.Dead
        && !movementStatusManager.Stunned
        && !movementStatusManager.MovementLocked;

    public PlayerMovementManager(Player player, Subject updateSubject, NavMeshAgent navMeshAgent)
        : base(player, navMeshAgent)
    {
        this.player = player;

        updateSubject.Subscribe(UpdateMovement);
        updateSubject.Subscribe(LookAtMouse);
        movementStatusManager = new MovementStatusManager(updateSubject);
        movementStatusManager.RegisterProviders(this, new StunHandler(player));

        player.AbilityAnimationListener.StartSubject.Subscribe(() => isCasting = true);
        player.AbilityAnimationListener.FinishSubject.Subscribe(() => isCasting = false);
        player.InterruptSubject.Subscribe(() => isCasting = false);
    }

    public bool Stuns() => movementPaused || isCasting;

    public void RegisterMovementStatusProviders(params IMovementStatusProvider[] providers)
    {
        movementStatusManager.RegisterProviders(providers);
    }

    /// <summary>
    /// Move along the navmesh in the given direction unless rooted, stunned or movement locked.
    /// </summary>
    /// <param name="speed"> Defaults to the actors speed stat. </param>
    public void Move(Vector3 direction, float? speed = null)
    {
        if (player.Dead) return;

        if (Stunned || MovementLocked) return;

        if (direction == Vector3.zero) return;

        if (speed == null) speed = GetMoveSpeed();

        navMeshAgent.Move(direction.normalized * (Time.deltaTime * speed.Value));
    }

    /// <summary>
    /// Lock movement velocity for a given duration with a fixed rotation.
    /// </summary>
    /// <param name="rotation">The rotation to maintain for the duration.</param>
    public bool LockMovement(float duration, float speed, Vector3 direction, Vector3 rotation)
    {
        movementStatusManager.LockMovement(duration);

        movementLockSpeed = speed;
        movementLockDirection = direction.normalized;

        if (rotation != Vector3.zero) Look(rotation);

        return true;
    }

    private void UpdateMovement()
    {
        if (player.Dead) return;

        navMeshAgent.speed = GetMoveSpeed();

        if (movementStatusManager.MovementLocked)
        {
            navMeshAgent.Move(movementLockDirection * (Time.deltaTime * movementLockSpeed));
        }
    }

    private void LookAtMouse()
	{
		Vector3 desiredLookatPosition = Vector3.zero;
		MouseGamePositionFinder.Instance.GetPlanePositions(player.transform.position.y, out desiredLookatPosition, out _);
        Vector3 targetPosition = desiredLookatPosition - player.transform.position;

        RotateTowards(targetPosition);

// 		if (IsFacingTarget(targetPosition, null))
// 		{
//             rotationSpeedMultiplier = Mathf.Clamp(rotationSpeedMultiplier - (player.RotationSpeedAcceleration * Time.deltaTime), 1, float.MaxValue);
// 		}
// 		else
// 		{
//             rotationSpeedMultiplier = Mathf.Clamp(rotationSpeedMultiplier + (player.RotationSpeedAcceleration * Time.deltaTime), 1, float.MaxValue);
// 		}
	}
}
