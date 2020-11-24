public class AwaitFeedbackProcessor : Processor<ComboState>
{
    private Player player;
    private readonly float feedbackTimeout;

    public AwaitFeedbackProcessor(Player player, float feedbackTimeout)
    {
        this.player = player;
        this.feedbackTimeout = feedbackTimeout;
    }

    public override void Enter()
    {
        throw new System.NotImplementedException();
    }

    public override void Exit()
    {
        throw new System.NotImplementedException();
    }

    public override bool TryCompleteProcess(out ComboState nextState)
    {
        throw new System.NotImplementedException();
    }
}