using UnityEngine;

public class TelegraphAttack : IStateMachineComponent
{
    private readonly Enemy enemy;
    private readonly Color telegraphColour;

    public TelegraphAttack(Enemy enemy, Color telegraphColour)
    {
        this.enemy = enemy;
        this.telegraphColour = telegraphColour;
    }

    public virtual void Enter()
    {
        enemy.MovementManager.StopPathfinding();
        enemy.MovementManager.ClearWatch();
        enemy.StartTelegraph(telegraphColour);
    }

    public virtual void Exit()
    {
        enemy.StopTelegraph();
    }

    public void Update() {}
}
