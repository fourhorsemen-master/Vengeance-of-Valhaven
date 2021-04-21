using UnityEngine;

public class TelegraphAttackAndWatch : IStateMachineComponent
{
    private readonly Enemy enemy;
    private readonly Actor target;
    private readonly Color telegraphColour;

    public TelegraphAttackAndWatch(Enemy enemy, Actor target, Color telegraphColour)
    {
        this.enemy = enemy;
        this.target = target;
        this.telegraphColour = telegraphColour;
    }

    public void Enter()
    {
        enemy.MovementManager.StopPathfinding();
        enemy.MovementManager.Watch(target.transform);
        enemy.StartTelegraph(telegraphColour);
    }

    public void Exit()
    {
        enemy.MovementManager.ClearWatch();
        enemy.StopTelegraph();
    }

    public void Update() {}
}
