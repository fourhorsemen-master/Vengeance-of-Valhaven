public class ShrineExists : StateMachineTrigger
{
    public override void Activate() {}
    public override void Deactivate() {}
    public override bool Triggers() => AbilityShrine.Exists;
}
