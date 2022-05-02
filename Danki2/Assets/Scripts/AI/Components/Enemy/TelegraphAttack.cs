using UnityEngine;

public class TelegraphAttack : IStateMachineComponent
{
    private readonly Enemy enemy;
    private readonly Color telegraphColour;
    private readonly bool shouldPauseMovement;

    public TelegraphAttack(Enemy enemy, Color telegraphColour, bool shouldPauseMovement)
    {
        this.enemy = enemy;
        this.telegraphColour = telegraphColour;
        this.shouldPauseMovement = shouldPauseMovement;
    }

    public virtual void Enter()
    {
		if (shouldPauseMovement)
		{
            enemy.MovementManager.Pause(null);
		}

        enemy.MovementManager.StopPathfinding();
        enemy.StartTelegraph(telegraphColour);
    }

    public virtual void Exit()
    {
        enemy.MovementManager.Unpause();
        enemy.StopTelegraph();
    }

    public void Update() {}
}
