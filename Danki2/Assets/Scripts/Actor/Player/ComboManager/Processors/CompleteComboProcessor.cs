﻿public class CompleteComboProcessor : Processor<ComboState>
{
    private readonly Player player;

    public CompleteComboProcessor(Player player)
    {
        this.player = player;
    }

    public void Enter()
    {
        player.AbilityTree.Reset();
    }

    public void Exit()
    {
    }

    public bool TryCompleteProcess(out ComboState newState)
    {
        newState = ComboState.LongCooldown;
        return true;
    }
}