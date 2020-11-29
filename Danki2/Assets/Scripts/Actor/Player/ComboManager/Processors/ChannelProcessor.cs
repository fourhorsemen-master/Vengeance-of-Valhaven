public class ChannelProcessor : Processor<ComboState>
{
    private Player player;
    private Direction direction;
    private ActionControlState previousActionControlState;

    public ChannelProcessor(Player player)
    {
        this.player = player;
    }

    public override void Enter()
    {
        previousActionControlState = PlayerControls.Instance.ActionControlState;
        direction = player.AbilityTree.DirectionLastWalked;
    }

    public override void Exit()
    {
    }

    public override bool TryCompleteProcess(out ComboState nextState)
    {
        ActionControlState currentActionControlState = PlayerControls.Instance.ActionControlState;

        CastingStatus currentCastingStatus = direction == Direction.Left
            ? CastingStatus.ChannelingLeft
            : CastingStatus.ChannelingRight;

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