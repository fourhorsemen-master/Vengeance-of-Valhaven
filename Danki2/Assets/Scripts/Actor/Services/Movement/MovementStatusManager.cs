using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MovementStatusManager
{
	private readonly List<MovementStatusProvider> statusProviders = new List<MovementStatusProvider>();
	private float movementLockRemainingDuration = 0f;

	public bool MovementLocked => movementLockRemainingDuration > 0;

	public bool Stunned => statusProviders.Any(p => p.SetStunned());

	public bool Rooted => statusProviders.Any(p => p.SetRooted());

	public MovementStatusManager(Subject updateSubject)
	{
		updateSubject.Subscribe(TickMovementLock);
	}

	public void RegisterProviders(params MovementStatusProvider[] providers)
	{
		statusProviders.AddRange(providers);
	}

	/// <summary>
	/// Return true if the movement lock is applied.
	/// </summary>
	/// <param name="overrideLock"></param>
	/// <param name="duration"></param>
	/// <returns></returns>
	public bool TryLockMovement(bool overrideLock, float duration)
	{
		if (!overrideLock && (Stunned || Rooted || MovementLocked)) return false;

		movementLockRemainingDuration = duration;
		return true;
	}

	private void TickMovementLock()
	{
		movementLockRemainingDuration = Mathf.Max(movementLockRemainingDuration - Time.deltaTime, 0);
	}
}
