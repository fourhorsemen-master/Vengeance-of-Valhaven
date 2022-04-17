using UnityEngine;
using UnityEngine.AI;

public interface IMovementManager
{
    public bool IsFacingTarget(Vector3 targetPosition, float? graceAngleOverride);
}

public abstract class MovementManager : IMovementManager
{
    private readonly Actor actor;
    protected readonly NavMeshAgent navMeshAgent;

    private const float WalkSpeedMultiplier = 0.3f;

    protected float rotationSpeedMultiplier = 1f;
    protected float? rotationSmoothingOverride = null;
    protected Transform watchTarget = null;
    protected bool watching = false;

    protected bool movementPaused = false;
    private Coroutine endPauseCoroutine = null;

    public MotionType MotionType { get; set; } = MotionType.Run;

    protected abstract float RotationSmoothing { get; }

    public MovementManager(Actor actor, NavMeshAgent navMeshAgent)
    {
        this.actor = actor;

        this.navMeshAgent = navMeshAgent;
    }

    /// <summary>
    /// Snap rotate the actor to face the position.
    /// </summary>
    public void LookAt(Vector3 position)
    {
        Look(position - actor.transform.position);
    }

    public bool IsFacingTarget(Vector3 targetPosition, float? graceAngleOverride)
	{
        Quaternion targetRotation = Quaternion.LookRotation(targetPosition);
        float angleDelta = Quaternion.Angle(actor.transform.rotation, targetRotation);
        float granceAngle = graceAngleOverride.HasValue ? graceAngleOverride.Value : actor.FacingAngleGrace;

        bool result = angleDelta <= granceAngle;

        Debug.Log("IsFacingTarget: Angle:" + angleDelta + ". Grace:" + granceAngle + ". Result:" + result);
        return result;
    }

	public bool CanStrafeTarget(Vector3 targetPosition)
	{
		Quaternion targetRotation = Quaternion.LookRotation(targetPosition);
		return Quaternion.Angle(actor.transform.rotation, targetRotation) <= actor.StrafeAngleLimit;
	}

	/// <summary>
	/// Lock the position and rotation for the given duration.
	/// </summary>
	public void Pause(float duration)
    {
        movementPaused = true;

        if (endPauseCoroutine != null) actor.StopCoroutine(endPauseCoroutine);

        endPauseCoroutine = actor.WaitAndAct(duration, () => {
            movementPaused = false;
            endPauseCoroutine = null;
        });
    }

    protected void Look(Vector3 rotation)
    {
        rotation.y = 0;
        actor.transform.rotation = Quaternion.LookRotation(rotation);
    }

    protected void RotateTowards(Vector3 direction)
    {
        if (direction.Equals(Vector3.zero)) return;

        float currentTurnSpeed = (actor.TurnSpeed * rotationSpeedMultiplier) * Time.deltaTime;
        
        Quaternion desiredRotation = Quaternion.LookRotation(direction);
        actor.transform.rotation = Quaternion.RotateTowards(actor.transform.rotation, desiredRotation, currentTurnSpeed);
    }

    protected float GetMoveSpeed()
    {
        float moveSpeed = actor.Speed;

        return MotionType == MotionType.Run
            ? moveSpeed
            : moveSpeed * WalkSpeedMultiplier;
    }
}
