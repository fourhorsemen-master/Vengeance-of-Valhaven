public class AtRootTrigger : StateMachineTrigger
{
    private readonly Player player;

    public AtRootTrigger(Player player)
    {
        this.player = player;
    }

    public override void Activate() { }

    public override void Deactivate() { }

    public override bool Triggers()
    {
        return player.AbilityTree.AtRoot;
    }
}