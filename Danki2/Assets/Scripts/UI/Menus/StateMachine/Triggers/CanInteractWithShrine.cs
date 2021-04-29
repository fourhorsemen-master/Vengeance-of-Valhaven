public class CanInteractWithShrine : StateMachineTrigger
{
    private readonly IShrine shrine;

    public CanInteractWithShrine(IShrine shrine)
    {
        this.shrine = shrine;
    }

    public override void Activate() {}
    public override void Deactivate() {}
    public override bool Triggers() => shrine.CanInteract();
}
