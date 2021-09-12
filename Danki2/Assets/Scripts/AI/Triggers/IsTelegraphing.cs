public class IsTelegraphing : StateMachineTrigger
{
    private Enemy enemy;

    public IsTelegraphing(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public override void Activate() { }

    public override void Deactivate() { }

    public override bool Triggers() => enemy.CurrentTelegraph != null;
}