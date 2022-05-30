using UnityEngine;

public class TelegraphAttackAndWatch : IStateMachineComponent
{
    private readonly Enemy enemy;
    private readonly Actor target;
    private readonly Color telegraphColour;
    private readonly float? rotationSpeedMultiplier;

    public TelegraphAttackAndWatch(
        Enemy enemy,
        Actor target,
        Color telegraphColour,
        float? rotationSpeedMultiplier = null
    )
    {
        this.enemy = enemy;
        this.target = target;
        this.telegraphColour = telegraphColour;
        this.rotationSpeedMultiplier = rotationSpeedMultiplier;
    }

    public void Enter()
    {
        enemy.MovementManager.SetRotationTarget(target.transform, rotationSpeedMultiplier);
        enemy.StartTelegraph(telegraphColour);
    }

    public void Exit()
    {
        enemy.StopTelegraph();
    }

    public void Update() {}
}
