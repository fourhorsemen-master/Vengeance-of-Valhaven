using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MovementStatusManager
{
	private readonly List<IMovementStatusProvider> statusProviders = new List<IMovementStatusProvider>();
	private float movementLockRemainingDuration = 0f;

	public bool MovementLocked => movementLockRemainingDuration > 0;

	public bool Stunned => statusProviders.Any(p => p.Stuns());

	public MovementStatusManager(Subject updateSubject)
	{
		updateSubject.Subscribe(TickMovementLock);
	}

	public void RegisterProviders(params IMovementStatusProvider[] providers)
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
		if (!overrideLock && (Stunned || MovementLocked)) return false;

		movementLockRemainingDuration = duration;
		return true;
	}

	private void TickMovementLock()
	{
		movementLockRemainingDuration = Mathf.Max(movementLockRemainingDuration - Time.deltaTime, 0);
	}
}
