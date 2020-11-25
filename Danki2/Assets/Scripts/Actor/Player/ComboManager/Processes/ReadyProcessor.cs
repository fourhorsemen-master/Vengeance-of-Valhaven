public abstract class ReadyProcessor : Processor<ComboState>
{
    protected readonly Player player;
    private ActionControlState previousActionControlState;

    public ReadyProcessor(Player player)
    {
        this.player = player;
    }

    public override void Enter()
    {
        previousActionControlState = PlayerControls.Instance.ActionControlState;
    }

    public override void Exit()
    {
    }

    public override bool TryCompleteProcess(out ComboState nextState)
    {
        ActionControlState currentActionControlState = PlayerControls.Instance.ActionControlState;

        CastingCommand castingCommand = ControlMatrix.GetCastingCommand(
            CastingStatus.Ready,
            previousActionControlState,
            currentActionControlState
        );

        switch (castingCommand)
        {
            case CastingCommand.CastLeft:
                nextState = ComboState.CastRight;
                return true;
            case CastingCommand.CastRight:
                nextState = ComboState.CastRight;
                return true;
            default:
                nextState = default;
                return false;
        }
    }
}
