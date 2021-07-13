using UnityEngine;

public class WolfPounce : IStateMachineComponent
{
    private readonly Wolf wolf;
    private readonly Actor target;

    public WolfPounce(Wolf wolf, Actor target)
    {
        this.wolf = wolf;
        this.target = target;
    }

    public void Enter() => wolf.Pounce(GetPounceTargetPosition());
    public void Exit() {}
    public void Update() {}

    private Vector3 GetPounceTargetPosition()
    {
        Vector3 targetPosition = target.transform.position;
        return targetPosition + (wolf.transform.position - targetPosition).normalized;
    }
}
