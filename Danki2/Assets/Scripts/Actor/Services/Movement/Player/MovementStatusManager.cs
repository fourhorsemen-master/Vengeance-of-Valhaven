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

	public void LockMovement(float duration)
	{
		movementLockRemainingDuration = duration;
	}

	private void TickMovementLock()
	{
		movementLockRemainingDuration = Mathf.Max(movementLockRemainingDuration - Time.deltaTime, 0);
	}
}
