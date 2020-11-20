public class AwaitFeedback : IStateMachineComponent
{
    private Player player;

    public AwaitFeedback(Player player)
    {
        this.player = player;
    }

    public void Enter()
    {
    }

    public void Exit()
    {
    }

    public void Update()
    {
        if (player.FeedbackSinceLastCast == null) return;

        bool feedback = player.FeedbackSinceLastCast.Value;

        if (feedback == false)
        {
            player.FailCombo();
        }
        else if (player.AbilityTree.CanWalk())
        {
            player.ContinueCombo();
        }
        else
        {
            player.CompleteCombo();
        }
    }
}