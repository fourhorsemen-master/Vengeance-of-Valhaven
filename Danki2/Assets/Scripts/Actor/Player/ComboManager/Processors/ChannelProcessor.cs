public class ChannelProcessor : Processor<ComboState>
{
    private readonly Player player;
    private ActionControlState previousActionControlState;
    private CastingStatus currentCastingStatus;

    public ChannelProcessor(Player player)
    {
        this.player = player;
    }

    public void Enter()
    {
        previousActionControlState = PlayerControls.Instance.ActionControlState;

        currentCastingStatus = player.AbilityTree.DirectionLastWalked == Direction.Left
            ? CastingStatus.ChannelingLeft
            : CastingStatus.ChannelingRight;
    }

    public void Exit()
    {
    }

    public bool TryCompleteProcess(out ComboState nextState)
    {
        ActionControlState currentActionControlState = PlayerControls.Instance.ActionControlState;

        CastingCommand castingCommand = ControlMatrix.GetCastingCommand(
            currentCastingStatus,
            previousActionControlState,
            currentActionControlState
        );

        previousActionControlState = currentActionControlState;

        if (castingCommand == CastingCommand.CancelChannel)
        {
            player.ChannelService.CancelChannel();
        }

        if (!player.ChannelService.Active)
        {
            nextState = ComboState.AwaitingFeedback;
            return true;
        }

        nextState = default;
        return false;
    }
}