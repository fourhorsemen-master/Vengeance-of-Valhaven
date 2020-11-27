public class FailComboProcessor : Processor<ComboState>
{
    private readonly Player player;

    public FailComboProcessor(Player player)
    {
        this.player = player;
    }

    public override void Enter()
    {
        player.AbilityTree.Reset();
        player.PlayWhiffSound();
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