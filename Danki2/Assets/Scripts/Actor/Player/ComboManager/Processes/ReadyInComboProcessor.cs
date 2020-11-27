using UnityEngine;

public class ReadyInComboProcessor : ReadyProcessor
{
    private readonly float feedbackTimeout;
    private float timeoutRemaining;

    public ReadyInComboProcessor(Player player, float feedbackTimeout) : base(player)
    {
        this.feedbackTimeout = feedbackTimeout;
    }

    public override void Enter()
    {
        base.Enter();
        timeoutRemaining = feedbackTimeout;
    }

    public override bool TryCompleteProcess(out ComboState newState)
    {
        if (base.TryCompleteProcess(out newState)) return true;

        timeoutRemaining -= Time.deltaTime;

        if (timeoutRemaining <= 0)
        {
            newState = ComboState.Whiff;
            return true;
        }

        newState = default;
        return false;
    }
}