public abstract class ReadyProcessor : Processor<ComboState>
{
    protected readonly Player player;

    public ReadyProcessor(Player player)
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
        throw new System.NotImplementedException();
    }
}
