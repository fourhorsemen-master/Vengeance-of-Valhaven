﻿public class AwaitFeedbackProcessor : Processor<ComboState>
{
    private Player player;

    public AwaitFeedbackProcessor(Player player)
    {
        this.player = player;
    }

    public void Enter()
    {
    }

    public void Exit()
    {
    }

    public bool TryCompleteProcess(out ComboState nextState)
    {
        switch (player.FeedbackSinceLastCast)
        {
            case true:
                nextState = player.AbilityTree.CanWalk()
                    ? ComboState.ContinueCombo
                    : ComboState.CompleteCombo;
                return true;

            case false:
                nextState = ComboState.FailCombo;
                return true;
        }

        nextState = default;
        return false;
    }
}