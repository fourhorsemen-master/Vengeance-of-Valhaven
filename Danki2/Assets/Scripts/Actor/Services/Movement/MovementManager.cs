using UnityEngine;
using UnityEngine.AI;

public abstract class MovementManager
{
    private readonly Actor actor;
    protected readonly NavMeshAgent navMeshAgent;

    private const float WalkSpeedMultiplier = 0.3f;

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

    public bool IsFacingTarget() => true; 

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
        
        Quaternion desiredRotation = Quaternion.LookRotation(direction);
        actor.transform.rotation = Quaternion.RotateTowards(actor.transform.rotation, desiredRotation, actor.TurnSpeed * Time.deltaTime );
    }

    protected float GetMoveSpeed()
    {
        float moveSpeed = actor.Speed;

        return MotionType == MotionType.Run
            ? moveSpeed
            : moveSpeed * WalkSpeedMultiplier;
    }
}
