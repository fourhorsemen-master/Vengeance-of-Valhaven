﻿public class AwaitFeedbackProcessor : Processor<ComboState>
{
    private Player player;

    public AwaitFeedbackProcessor(Player player)
    {
        this.player = player;
    }

    public override void Enter()
    {
    }

    public override void Exit()
    {
    }

    public override bool TryCompleteProcess(out ComboState nextState)
    {
        if (player.FeedbackSinceLastCast == true)
        {
            nextState = player.AbilityTree.CanWalk()
                ? ComboState.ContinueCombo
                : ComboState.CompleteCombo;
            return true;
        }
        else if (player.FeedbackSinceLastCast == false)
        {
            nextState = ComboState.FailCombo;
            return true;
        }

        nextState = default;
        return false;
    }
}