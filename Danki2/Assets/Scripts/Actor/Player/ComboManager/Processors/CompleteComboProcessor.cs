public class CompleteComboProcessor : Processor<ComboState>
{
    private readonly Player player;

    public CompleteComboProcessor(Player player)
    {
        this.player = player;
    }

    public override void Enter()
    {
        player.AbilityTree.Reset();
    }

    public override void Exit()
    {
    }

    public override bool TryCompleteProcess(out ComboState newState)
    {
        newState = ComboState.LongCooldown;
        return true;
    }
}