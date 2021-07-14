using UnityEngine;

public class ShortCooldownProcessor : Processor<ComboState>
{
    private readonly Player player;
    private readonly float shortCooldown;
    private readonly bool continuousCombo;
    private bool canPrecast = true;
    private float cooldownExpiry;
    private ActionControlState previousActionControlState;

    public ShortCooldownProcessor(Player player, float shortCooldown, bool continuousCombo)
    {
        this.player = player;
        this.shortCooldown = shortCooldown;
        this.continuousCombo = continuousCombo;
    }

    public void Enter()
    {
        cooldownExpiry = Time.time + shortCooldown;
        previousActionControlState = PlayerControls.Instance.ActionControlState;
        if (!continuousCombo) canPrecast = false;
    }

    public void Exit()
    {
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

        if (Time.time >= cooldownExpiry)
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