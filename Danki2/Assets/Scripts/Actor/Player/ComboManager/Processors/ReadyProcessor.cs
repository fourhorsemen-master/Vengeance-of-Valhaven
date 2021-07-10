public abstract class ReadyProcessor : Processor<ComboState>
{
    private readonly Player player;
    private ActionControlState previousActionControlState;

    protected ReadyProcessor(Player player)
    {
        this.player = player;
    }

    public virtual void Enter()
    {
        previousActionControlState = PlayerControls.Instance.ActionControlState;
    }

    public virtual void Exit()
    {
    }

    public virtual bool TryCompleteProcess(out ComboState nextState)
    {
        ActionControlState currentActionControlState = PlayerControls.Instance.ActionControlState;

        CastingCommand castingCommand = ControlMatrix.GetCastingCommand(
            CastingStatus.Ready,
            previousActionControlState,
            currentActionControlState
        );

        previousActionControlState = currentActionControlState;

        Direction castDirection;

        switch (castingCommand)
        {
            case CastingCommand.CastLeft:
                castDirection = Direction.Left;
                break;
            case CastingCommand.CastRight:
                castDirection = Direction.Right;
                break;
            default:
                nextState = default;
                return false;
        }

        if (!player.AbilityTree.CanWalkDirection(castDirection) || !player.CanCast)
        {
            nextState = default;
            return false;
        }

        nextState = castDirection switch
        {
            Direction.Left => ComboState.CastLeft,
            Direction.Right => ComboState.CastRight,
            _ => default
        };

        return true;
    }
}
