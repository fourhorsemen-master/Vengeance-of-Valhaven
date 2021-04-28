using UnityEngine;

public class TelegraphAttackAndWatch : IStateMachineComponent
{
    private readonly Enemy enemy;
    private readonly Actor target;
    private readonly Color telegraphColour;
    private readonly float? rotationSmoothingOverride;

    public TelegraphAttackAndWatch(
        Enemy enemy,
        Actor target,
        Color telegraphColour,
        float? rotationSmoothingOverride = null
    )
    {
        this.enemy = enemy;
        this.target = target;
        this.telegraphColour = telegraphColour;
        this.rotationSmoothingOverride = rotationSmoothingOverride;
    }

    public void Enter()
    {
        enemy.MovementManager.Watch(target.transform, rotationSmoothingOverride);
        enemy.StartTelegraph(telegraphColour);
    }

    public void Exit()
    {
        enemy.MovementManager.ClearWatch();
        enemy.StopTelegraph();
    }

    public void Update() {}
}
