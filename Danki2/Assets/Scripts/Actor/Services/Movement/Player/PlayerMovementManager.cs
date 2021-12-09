using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovementManager : MovementManager, IMovementStatusProvider
{
    private static readonly ISet<MovementLockType> MoveLockOverrideTypes = new HashSet<MovementLockType>
    {
        MovementLockType.Knockback,
        MovementLockType.Pull,
        MovementLockType.AbilityDash
    };

    private static readonly ISet<MovementLockType> MovementLockTypesAffectedByWeight = new HashSet<MovementLockType>
    {
        MovementLockType.Knockback,
        MovementLockType.Pull
    };

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

        RotateTowards(direction);

        if (speed == null) speed = GetMoveSpeed();

        navMeshAgent.Move(direction.normalized * (Time.deltaTime * speed.Value));
    }

    public bool TryLockMovement(MovementLockType type, float duration, float speed, Vector3 direction, Vector3 rotation)
    {
        bool overrideLock = MoveLockOverrideTypes.Contains(type);

        if (MovementLockTypesAffectedByWeight.Contains(type)) speed /= player.Weight;

        return TryLockMovement(overrideLock, duration, speed, direction, rotation);
    }

    /// <summary>
    /// Lock movement velocity for a given duration with a fixed rotation.
    /// </summary>
    /// <param name="rotation">The rotation to maintain for the duration.</param>
    private bool TryLockMovement(bool overrideLock, float duration, float speed, Vector3 direction, Vector3 rotation)
    {
        if (!movementStatusManager.TryLockMovement(overrideLock, duration)) return false;

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
}
