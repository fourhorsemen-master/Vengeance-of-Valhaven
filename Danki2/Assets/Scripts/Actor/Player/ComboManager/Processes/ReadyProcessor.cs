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
                bool hasCast = player.InstantCastService.TryCast(
                    abilityReference,
                    player.TargetFinder.FloorTargetPosition,
                    player.TargetFinder.OffsetTargetPosition,
                    player.TargetFinder.Target
                );
                if (hasCast)
                {
                    player.AbilityTree.Walk(castDirection);
                    nextState = ComboState.AwaitingFeedback;
                    return true;
                }
                break;
            case AbilityType.Channel:
                bool hasStartedChannel = player.ChannelService.TryStartChannel(abilityReference);
                if (hasStartedChannel)
                {
                    player.AbilityTree.Walk(castDirection);
                    nextState = castDirection == Direction.Left
                        ? ComboState.ChannelingLeft
                        : ComboState.ChannelingRight;
                    return true;
                }
                break;
        }

        nextState = player.AbilityTree.AtRoot ? ComboState.ReadyAtRoot : ComboState.ReadyInCombo;
        return false;
    }
}
