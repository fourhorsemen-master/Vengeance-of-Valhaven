﻿public class WolfPounce : StateMachineComponent
{
    private readonly Wolf wolf;
    private readonly Actor target;

    public WolfPounce(Wolf wolf, Actor target)
    {
        this.wolf = wolf;
        this.target = target;
    }

    public void Enter() => wolf.Pounce(target.transform.position);
    public void Exit() {}
    public void Update() {}
}
