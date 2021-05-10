public class CanInteractWithShrine<T> : StateMachineTrigger where T : Singleton<T>, IShrine
{
    private readonly T shrine;

    public CanInteractWithShrine(T shrine)
    {
        this.shrine = shrine;
    }

    public override void Activate() {}
    public override void Deactivate() {}
    public override bool Triggers() => Singleton<T>.Exists && shrine.CanInteract();
}
