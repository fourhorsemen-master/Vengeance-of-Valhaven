public class AwaitAbilityCompletionProcessor : Processor<ComboState>
{
    private readonly Player player;
    private readonly AbilityAnimationListener abilityAnimationListener;
    private bool canPrecast = true;
    private ActionControlState previousActionControlState;
    private Subscription finishAbilitySubscription;
    private bool abilityFinished = false;

    public AwaitAbilityCompletionProcessor(Player player, AbilityAnimationListener abilityAnimationListener)
    {
        this.player = player;
        this.abilityAnimationListener = abilityAnimationListener;
    }

    public void Enter()
    {
        previousActionControlState = PlayerControls.Instance.ActionControlState;
        finishAbilitySubscription = abilityAnimationListener.FinishSubject.Subscribe(() => abilityFinished = true);
        canPrecast = false;
        abilityFinished = false;
    }

    public void Exit()
    {
        finishAbilitySubscription.Unsubscribe();
    }

    public bool TryCompleteProcess(out ComboState newState)
    {
        ActionControlState currentActionControlState = PlayerControls.Instance.ActionControlState;

        CastingCommand castingCommand = ControlMatrix.GetCastingCommand(
            CastingStatus.Cooldown,
            previousActionControlState,
            currentActionControlState
        );

        // We need to have let go of the control for the last cast in order to precast the next ability
        if (castingCommand == CastingCommand.None) canPrecast = true;

        previousActionControlState = currentActionControlState;

        if (abilityFinished)
        {
            newState = ComboState.ReadyInCombo;

            switch (castingCommand)
            {
                case CastingCommand.PrecastLeft:
                    if (canPrecast && player.AbilityTree.CanWalkDirection(Direction.Left) && player.CanCast)
                        newState = ComboState.CastLeft;
                    break;
                case CastingCommand.PrecastRight:
                    if (canPrecast && player.AbilityTree.CanWalkDirection(Direction.Right) && player.CanCast)
                        newState = ComboState.CastRight;
                    break;
            }

            return true;
        }

        newState = default;
        return false;
    }
}