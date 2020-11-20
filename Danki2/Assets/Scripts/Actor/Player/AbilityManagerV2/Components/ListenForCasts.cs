internal class ListenForCasts : IStateMachineComponent
{
    private Player player;
    private ActionControlState actionControlState;

    public ListenForCasts(Player player)
    {
        this.player = player;
    }

    public void Enter()
    {
        player.Ready();
        actionControlState = PlayerControls.Instance.ActionControlState;
    }

    public void Exit() { }

    public void Update()
    {
        var newActionControlState = PlayerControls.Instance.ActionControlState;
        var castingCommand = ControlMatrix.GetCastingCommand(CastingStatus.Ready, actionControlState, newActionControlState);
        actionControlState = newActionControlState;
        
        if (castingCommand == CastingCommand.CastLeft)
        {
            player.Cast(Direction.Left);
        }
        else if (castingCommand == CastingCommand.CastRight)
        {
            player.Cast(Direction.Right);
        }
    }
}