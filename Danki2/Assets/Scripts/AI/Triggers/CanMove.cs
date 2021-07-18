public class CanMove : StateMachineTrigger
{
    private readonly Enemy enemy;

    public CanMove(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public override void Activate() { }

    public override void Deactivate() { }

    public override bool Triggers() => enemy.MovementManager.CanMove;
}