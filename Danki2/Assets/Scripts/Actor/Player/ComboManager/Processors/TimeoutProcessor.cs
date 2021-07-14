public class TimeoutProcessor : Processor<ComboState>
{
    private readonly Player player;

    public TimeoutProcessor(Player player)
    {
        this.player = player;
    }

    public void Enter()
    {
        player.AbilityTree.Reset();
    }

    public void Exit()
    {
    }

    public bool TryCompleteProcess(out ComboState newState)
    {
        newState = ComboState.TimeoutCooldown;
        return true;
    }
}