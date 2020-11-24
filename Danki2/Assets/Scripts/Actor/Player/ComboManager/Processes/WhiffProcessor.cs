public class WhiffProcessor : Processor<ComboState>
{
    private Player player;

    public WhiffProcessor(Player player)
    {
        this.player = player;
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
        nextState = ComboState.LongCooldown;
        return true;
    }
}