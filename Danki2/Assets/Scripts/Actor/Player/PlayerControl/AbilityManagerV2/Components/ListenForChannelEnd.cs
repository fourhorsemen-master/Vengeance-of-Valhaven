internal class ListenForChannelEnd : IStateMachineComponent
{
    private Player player;
    private CastingStatus castingStatus;
    private ActionControlState actionControlState;

    public ListenForChannelEnd(Player player, Direction direction)
    {
        this.player = player;
        castingStatus = direction == Direction.Left ? CastingStatus.ChannelingLeft : CastingStatus.ChannelingRight;
    }

    public void Enter()
    {
        actionControlState = PlayerControls.Instance.ActionControlState;
    }

    public void Exit() { }

    public void Update()
    {
        var newActionControlState = PlayerControls.Instance.ActionControlState;
        var newCastingCommand = ControlMatrix.GetCastingCommand(castingStatus, actionControlState, newActionControlState);

        if (newCastingCommand == CastingCommand.CancelChannel)
        {
            player.ChannelService.CancelChannel();
        }
    }
}