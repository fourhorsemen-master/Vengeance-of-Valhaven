public abstract class ReadyProcessor : Processor<ComboState>
{
    private readonly Player player;
    private ActionControlState previousActionControlState;

    protected ReadyProcessor(Player player)
    {
        this.player = player;
    }

    public void Enter()
    {
        previousActionControlState = PlayerControls.Instance.ActionControlState;
    }

    public void Exit()
    {
    }

    public bool TryCompleteProcess(out ComboState nextState)
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

        AbilityReference abilityReference = player.AbilityTree.GetAbility(castDirection);
        AbilityType abilityType = AbilityLookup.Instance.GetAbilityType(abilityReference);

        switch (abilityType)
        {
            case AbilityType.InstantCast:
                if (!player.InstantCastService.CanCast) break;
                player.AbilityTree.Walk(castDirection);
                player.InstantCastService.TryCast(
                    abilityReference,
                    player.TargetFinder.FloorTargetPosition,
                    player.TargetFinder.OffsetTargetPosition,
                    player.TargetFinder.Target
                );
                nextState = ComboState.AwaitingFeedback;
                return true;
            case AbilityType.Channel:
                if (!player.ChannelService.CanCast) break;
                player.AbilityTree.Walk(castDirection);
                player.ChannelService.TryStartChannel(abilityReference);
                nextState =  ComboState.Channeling;
                return true;
        }

        nextState = default;
        return false;
    }
}
