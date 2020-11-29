public class FailComboProcessor : Processor<ComboState>
{
    private readonly Player player;

    public FailComboProcessor(Player player)
    {
        this.player = player;
    }

    public void Enter()
    {
        player.AbilityTree.Reset();
        player.PlayWhiffSound();
    }

    public void Exit()
    {
    }

    public bool TryCompleteProcess(out ComboState newState)
    {
        newState = ComboState.LongCooldown;
        return true;
    }
}