public class EndComboProcessor : Processor<ComboState>
{
    private readonly Player player;
    bool skipframe = false;

    public EndComboProcessor(Player player)
    {
        this.player = player;
    }

    public void Enter()
    {
        player.AbilityTree.Reset();
        skipframe = true;
    }

    public void Exit()
    {
    }

    public bool TryCompleteProcess(out ComboState newState)
    {
        if (!skipframe && player.IsCurrentAnimationState(CommonAnimStrings.Locomotion))
        {
            newState = ComboState.ReadyAtRoot;
            return true;
        }

        skipframe = false;
        newState = default;
        return false;
    }
}