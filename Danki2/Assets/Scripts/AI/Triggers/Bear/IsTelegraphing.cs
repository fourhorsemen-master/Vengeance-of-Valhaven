public class IsTelegraphing : StateMachineTrigger
{
    private Bear bear;

    public IsTelegraphing(Bear bear)
    {
        this.bear = bear;
    }

    public override void Activate() { }

    public override void Deactivate() { }

    public override bool Triggers() => bear.CurrentTelegraph != null;
}