public class NeverTrigger : StateMachineTrigger
{
    public override void Activate() {}
    public override void Deactivate() {}
    public override bool Triggers() => false;
}
