public class CastProcessor : Processor<ComboState>
{
    private readonly Player player;
    private readonly Direction castDirection;

    public CastProcessor(Player player, Direction castDirection)
    {
        this.player = player;
        this.castDirection = castDirection;
    }

    public void Enter()
    {
    }

    public void Exit()
    {
    }

    public bool TryCompleteProcess(out ComboState nextState)
    {
        player.AbilityService.Cast(castDirection, player.TargetFinder.FloorTargetPosition);
        nextState = player.AbilityTree.CanWalk()
             ? ComboState.ShortCooldown
             : ComboState.CompleteCombo;
        return true;
    }
}
