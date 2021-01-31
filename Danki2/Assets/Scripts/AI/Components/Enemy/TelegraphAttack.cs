using UnityEngine;

public class TelegraphAttack : IStateMachineComponent
{
    private readonly Enemy enemy;
    private readonly float telegraphTime;
    private readonly Color telegraphColour;

    public TelegraphAttack(Enemy enemy, float telegraphTime, Color telegraphColour)
    {
        this.enemy = enemy;
        this.telegraphTime = telegraphTime;
        this.telegraphColour = telegraphColour;
    }

    public void Enter()
    {
        enemy.MovementManager.StopPathfinding();
        enemy.MovementManager.ClearWatch();
        enemy.Telegraph(telegraphTime, telegraphColour);
    }

    public void Exit() {}

    public void Update() {}
}
