public class AlwaysTrigger : AiTrigger
{
    public override void Activate() {}
    public override void Deactivate() {}
    public override bool Triggers() => true;
}
