public class ChannelProcessor : Processor<ComboState>
{
    private Player player;
    private Direction right;

    public ChannelProcessor(Player player, Direction right)
    {
        this.player = player;
        this.right = right;
    }

    public override void Enter()
    {
        throw new System.NotImplementedException();
    }

    public override void Exit()
    {
        throw new System.NotImplementedException();
    }

    public override bool TryCompleteProcess(out ComboState nextState)
    {
        throw new System.NotImplementedException();
    }
}