using UnityEngine;

public class MovementStatusManager
{
    private EnumDictionary<MovementStatus, float> remainingDurations = new EnumDictionary<MovementStatus, float>(0f);

	public bool Stunned => remainingDurations[MovementStatus.Stunned] > 0;

	public bool Rooted => remainingDurations[MovementStatus.Rooted] > 0;

	public bool MovementLocked => remainingDurations[MovementStatus.MovementLocked] > 0;

	public MovementStatusManager(Subject updateSubject)
	{
		updateSubject.Subscribe(TickStatuses);
	}

	public void Stun(float duration)
	{
		remainingDurations[MovementStatus.Stunned] = Mathf.Max(duration, remainingDurations[MovementStatus.Stunned]);
	}

	public void Root(float duration)
	{
		remainingDurations[MovementStatus.Rooted] = Mathf.Max(duration, remainingDurations[MovementStatus.Rooted]);
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

		remainingDurations[MovementStatus.MovementLocked] = duration;
		return true;
	}

	private void TickStatuses()
	{
		remainingDurations.ForEachKey(k =>
		{
			remainingDurations[k] = Mathf.Max(remainingDurations[k] - Time.deltaTime, 0);
		});
	}
}
