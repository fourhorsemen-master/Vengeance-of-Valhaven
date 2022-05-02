using UnityEngine;
using UnityEngine.AI;

public class EnemyMovementManager : MovementManager
{
	private readonly Enemy enemy;

	private const float DestinationTolerance = 0.5f;

	private bool movementLocked = false;
	private float movementLockSpeed;
	private Transform objectToFollow;
	private Vector3 pointToMoveTo = Vector3.zero;
	private Vector3 movementLockDirection;
	private Coroutine endMoveLockCoroutine;

	private bool watching = false;
	private bool rotationLocked = false;
	private Transform objectToWatch = null;
	private Vector3 pointToLookAt = Vector3.zero;
	private Coroutine endRotationLockCoroutine;

	private Subscription updateSubscription;
	private Subscription<DeathData> deathSubscription;

	protected override float RotationSmoothing => rotationSmoothingOverride ?? enemy.RotationSmoothing;

	public bool CanMove => !enemy.Dead && !movementPaused && !movementLocked;

	public EnemyMovementManager(Enemy enemy, Subject updateSubject, NavMeshAgent navMeshAgent)
		: base(enemy, navMeshAgent)
	{
		this.enemy = enemy;

		deathSubscription = this.enemy.DeathSubject.Subscribe(_ => OnDeath());
		updateSubscription = updateSubject.Subscribe(UpdateMovement);

		navMeshAgent.updateRotation = false;
	}

	public void SetMovementTargetPoint(Vector3 targetLocation)
	{
		pointToMoveTo = targetLocation;
		objectToFollow = null;
	}

	public void SetMovementTarget(Transform targetToFollow)
	{
		objectToFollow = targetToFollow;
	}

	private bool GetMovementTarget(out Vector3 positionToMoveTo)
	{
		if (objectToFollow)
		{
			positionToMoveTo = objectToFollow.position;
			return true;
		}

		if (pointToMoveTo != Vector3.zero)
		{
			positionToMoveTo = pointToMoveTo;
			return true;
		}

		positionToMoveTo = Vector3.zero;
		return false;
	}

	public void SetRotationTargetPoint(Vector3 targetToLookAt, float? rotationSpeedMultiplier)
	{
		objectToWatch = null;
		pointToLookAt = targetToLookAt;

		UpdateRotationSpeedMultiplier(rotationSpeedMultiplier);
	}

	public void SetRotationTarget(Transform targetToLookAt, float? rotationSpeedMultiplier)
	{
		objectToWatch = targetToLookAt;
		pointToLookAt = targetToLookAt ? targetToLookAt.position : Vector3.zero;

		UpdateRotationSpeedMultiplier(rotationSpeedMultiplier);
	}

	private void UpdateRotationSpeedMultiplier(float? rotationSpeedMultiplier)
	{
		if (rotationSpeedMultiplier.HasValue)
		{
			this.rotationSpeedMultiplier = rotationSpeedMultiplier.Value;
		}
		else
		{
			this.rotationSpeedMultiplier = baseRotationSpeedMultiplier;
		}
	}

	private bool GetRotationTarget(out Vector3 positionToLookAt)
	{
		if (objectToWatch) 
		{ 
			positionToLookAt = objectToWatch.position;
			return true;
		}

		if (pointToLookAt != Vector3.zero)
		{
			positionToLookAt = pointToLookAt;
			return true;
		}

		positionToLookAt = Vector3.zero;
		return false;
	}

	private void StartPathfinding(Vector3 destination)
	{
		if (!CanMove) return;

		navMeshAgent.isStopped = false;

		if (navMeshAgent.destination == destination) return;

		navMeshAgent.SetDestination(destination);
	}

	public bool CanPathToDestination(Vector3 destination, float tolerance = DestinationTolerance)
	{
		return NavMesh.SamplePosition(destination, out NavMeshHit hit, tolerance, NavMesh.AllAreas);
	}

	public void LockMovement(float duration, float speed, Vector3 direction, Vector3 rotation)
	{
		StopPathfinding();
		movementLocked = true;
		movementLockSpeed = speed;
		movementLockDirection = direction.normalized;

		if (rotation != Vector3.zero) Look(rotation);

		if (endMoveLockCoroutine != null)
		{
			enemy.StopCoroutine(endMoveLockCoroutine);
		}

		endMoveLockCoroutine = enemy.WaitAndAct(duration, () => { movementLocked = false; });
	}

	public void LockRotation(float? duration)
	{
		rotationLocked = true;

		if(endRotationLockCoroutine != null)
		{
			enemy.StopCoroutine(endRotationLockCoroutine);
		}

		if (duration.HasValue)
		{
			endRotationLockCoroutine = enemy.WaitAndAct(duration.Value, () => { rotationLocked = false; });
		}
	}

	public void UnlockRotation()
	{
		rotationLocked = false;

		if (endRotationLockCoroutine != null)
		{
			enemy.StopCoroutine(endRotationLockCoroutine);
		}
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
		if (movementPaused && !navMeshAgent.isStopped) StopPathfinding();

		navMeshAgent.speed = GetMoveSpeed();

		Vector3 desiredLookPostition;
		bool hasValidRotationTarget = GetRotationTarget(out desiredLookPostition);

		if (!rotationLocked && hasValidRotationTarget)
		{
			Vector3 turnVector = desiredLookPostition - enemy.transform.position;
			RotateTowards(turnVector);
			UpdateRotationAcceleration(IsFacingTarget(desiredLookPostition, null));
		}

		if (movementLocked)
		{
			Vector3 lockMoveOffest = movementLockDirection * (Time.deltaTime * movementLockSpeed);
			if (CanStrafeTarget(enemy.transform.position + lockMoveOffest))
			{
				navMeshAgent.Move(lockMoveOffest);
			}
		}
		else
		{
			if (movementPaused) return;

			Vector3 desiredMovementPostion;
			bool hasValidMovementTarget = GetMovementTarget(out desiredMovementPostion);

			if (hasValidMovementTarget)
			{
				Vector3 strafeVector = desiredMovementPostion - enemy.transform.position;
				//Debug.DrawLine(enemy.transform.position, desiredMovementPostion, Color.magenta);

				if (CanStrafeTarget(desiredMovementPostion))
				{
					StartPathfinding(desiredMovementPostion);
				}
			}
			else
			{
				StopPathfinding();
			}
		}
	}

	private void OnDeath()
	{
		StopPathfinding();
		updateSubscription.Unsubscribe();
	}
}
