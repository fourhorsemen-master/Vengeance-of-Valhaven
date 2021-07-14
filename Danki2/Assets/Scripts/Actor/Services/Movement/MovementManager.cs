using UnityEngine;
using UnityEngine.AI;

public abstract class MovementManager
{
    private readonly Actor actor;
    protected readonly NavMeshAgent navMeshAgent;

    private const float WalkSpeedMultiplier = 0.3f;

    protected bool movementPaused = false;
    private Coroutine endPauseCoroutine = null;

    public MotionType MotionType { get; set; } = MotionType.Run;

    protected abstract float RotationSmoothing { get; }
    public abstract bool CanMove { get; }

    public MovementManager(Actor actor, NavMeshAgent navMeshAgent)
    {
        this.actor = actor;

        this.navMeshAgent = navMeshAgent;
    }

    /// <summary>
    /// Snap rotate the actor to face the position.
    /// </summary>
    /// <param name="position"></param>
    public void LookAt(Vector3 position)
    {
        Look(position - actor.transform.position);
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

    protected void Look(Vector3 rotation)
    {
        rotation.y = 0;
        actor.transform.rotation = Quaternion.LookRotation(rotation);
    }

    protected void RotateTowards(Vector3 direction)
    {
        if (!direction.Equals(Vector3.zero))
        {
            Quaternion desiredRotation = Quaternion.LookRotation(direction);
            float lerpAmount = MathUtils.GetDeltaTimeLerpAmount(RotationSmoothing);
            actor.transform.rotation = Quaternion.Lerp(actor.transform.rotation, desiredRotation, lerpAmount);
        }
    }

    protected float GetMoveSpeed()
    {
        float moveSpeed = actor.StatsManager.Get(Stat.Speed);

        return MotionType == MotionType.Run
            ? moveSpeed
            : moveSpeed * WalkSpeedMultiplier;
    }
}
