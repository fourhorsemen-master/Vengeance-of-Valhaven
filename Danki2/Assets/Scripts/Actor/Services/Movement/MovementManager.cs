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

	protected float baseRotationSpeedMultiplier = 1f;
	protected float rotationSpeedMultiplier = 1f;
	protected float? rotationSmoothingOverride = null;

	protected bool movementPaused = false;
	private Coroutine endPauseCoroutine = null;

	public MotionType MotionType { get; set; } = MotionType.Run;

	protected abstract float RotationSmoothing { get; }

	public MovementManager(Actor actor, NavMeshAgent navMeshAgent)
	{
		this.actor = actor;

		this.navMeshAgent = navMeshAgent;
		baseRotationSpeedMultiplier = rotationSpeedMultiplier;
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
		Vector3 ourForward = actor.transform.forward;
		ourForward.y = 0f; //2D top-down check only;

		//Debug.DrawLine(actor.transform.position, actor.transform.position + ourForward, Color.green);

		Vector3 offsetToTarget = targetPosition - actor.transform.position;
		offsetToTarget.y = 0f;
		targetPosition.Normalize();

		//Debug.DrawLine(actor.transform.position, actor.transform.position + offsetToTarget, Color.yellow);

		float AngleDelta = Vector3.Angle(ourForward, offsetToTarget);
		float graceAngle = graceAngleOverride.HasValue ? graceAngleOverride.Value : actor.FacingAngleGrace;

		return AngleDelta <= graceAngle;
	}

	public bool CanStrafeTarget(Vector3 targetPosition)
	{
		if (actor.IsFreeStrafe) return true;

		Vector3 ourForward = actor.transform.forward;
		ourForward.y = 0f; //2D top-down check only;

		Debug.DrawLine(actor.transform.position, actor.transform.position + ourForward, Color.red);

		Vector3 offsetToTarget = targetPosition - actor.transform.position;
		offsetToTarget.y = 0f;
		offsetToTarget.Normalize();

		Debug.DrawLine(actor.transform.position, actor.transform.position + offsetToTarget, Color.magenta);

		float AngleDelta = Vector3.Angle(ourForward, offsetToTarget);

		return AngleDelta <= actor.StrafeAngleLimit;
	}

	/// <summary>
	/// Lock the position and rotation for the given duration.
	/// </summary>
	public void Pause(float? duration)
	{
		movementPaused = true;

		if (endPauseCoroutine != null) actor.StopCoroutine(endPauseCoroutine);

		if(duration.HasValue)
		{
			endPauseCoroutine = actor.WaitAndAct(duration.Value, () => {
				movementPaused = false;
				endPauseCoroutine = null;
			});
		}
	}

	public void Unpause()
	{
		movementPaused = false;

		if (endPauseCoroutine != null) actor.StopCoroutine(endPauseCoroutine);
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

	protected void UpdateRotationAcceleration(bool isFacingTarget)
	{
		if (isFacingTarget)
		{
			rotationSpeedMultiplier = Mathf.Clamp(rotationSpeedMultiplier - (actor.RotationSpeedAcceleration * Time.deltaTime), 1, float.MaxValue);
		}
		else
		{
			rotationSpeedMultiplier = Mathf.Clamp(rotationSpeedMultiplier + (actor.RotationSpeedAcceleration * Time.deltaTime), 1, float.MaxValue);
		}
	}
}
