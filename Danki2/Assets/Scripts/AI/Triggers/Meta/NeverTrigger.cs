public class NeverTrigger : AiTrigger
{
    public override void Activate() {}
    public override void Deactivate() {}
    public override bool Triggers() => false;
}
