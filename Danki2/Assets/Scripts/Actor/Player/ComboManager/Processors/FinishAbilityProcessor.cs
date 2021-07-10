public class FinishAbilityProcessor : Processor<ComboState>
{
    private readonly Player player;

    public FinishAbilityProcessor(Player player)
    {
        this.player = player;
    }

    public void Enter()
    {
    }

    public void Exit()
    {
    }

    public bool TryCompleteProcess(out ComboState nextState)
    {
        nextState = player.AbilityTree.CanWalk()
            ? ComboState.ContinueCombo
            : ComboState.CompleteCombo;

        return true;
    }
}