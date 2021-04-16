using UnityEngine;

public class WolfDashAway : IStateMachineComponent
{
    private readonly Wolf wolf;
    private readonly Actor target;

    public WolfDashAway(Wolf wolf, Actor target)
    {
        this.wolf = wolf;
        this.target = target;
    }

    public void Enter()
    {
        Vector3 direction = wolf.transform.position - target.transform.position;
        wolf.Dash(direction);
    }
    public void Exit() { }
    public void Update() { }
}
