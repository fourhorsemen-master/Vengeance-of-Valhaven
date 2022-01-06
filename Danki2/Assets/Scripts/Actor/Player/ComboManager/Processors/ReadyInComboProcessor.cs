using UnityEngine;

public class ReadyInComboProcessor : ReadyProcessor
{
    private readonly Player player;

    public ReadyInComboProcessor(Player player) : base(player)
    {
        this.player = player;
    }

    public override bool TryCompleteProcess(out ComboState newState)
    {
        if (base.TryCompleteProcess(out newState)) return true;

        AnimatorStateInfo currentState = player.AnimController.GetCurrentAnimatorStateInfo(0);

        if (currentState.IsName(CommonAnimStrings.Locomotion))
        {
            newState = ComboState.Timeout;
            return true;
        }

        newState = default;
        return false;
    }
}