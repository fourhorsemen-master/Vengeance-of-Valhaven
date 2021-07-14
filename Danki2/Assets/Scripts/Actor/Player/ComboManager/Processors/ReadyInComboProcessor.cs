using UnityEngine;

public class ReadyInComboProcessor : ReadyProcessor
{
    private readonly float feedbackTimeout;
    private float feedbackExpiry;

    public ReadyInComboProcessor(Player player, float feedbackTimeout) : base(player)
    {
        this.feedbackTimeout = feedbackTimeout;
    }

    public override void Enter()
    {
        base.Enter();
        feedbackExpiry = Time.time + feedbackTimeout;
    }

    public override bool TryCompleteProcess(out ComboState newState)
    {
        if (base.TryCompleteProcess(out newState)) return true;

        if (Time.time >= feedbackExpiry)
        {
            newState = ComboState.Timeout;
            return true;
        }

        newState = default;
        return false;
    }
}