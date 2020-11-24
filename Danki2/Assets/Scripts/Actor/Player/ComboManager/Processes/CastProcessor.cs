public class CastProcessor : Processor<ComboState>
{
    private Player player;
    private Direction left;

    public CastProcessor(Player player, Direction left)
    {
        this.player = player;
        this.left = left;
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