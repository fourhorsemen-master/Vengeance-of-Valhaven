public class RuneShrineExists : StateMachineTrigger
{
    public override void Activate() {}
    public override void Deactivate() {}
    public override bool Triggers() => RuneShrine.Exists;
}
