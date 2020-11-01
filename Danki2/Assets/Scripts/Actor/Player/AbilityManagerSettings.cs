public class AbilityManagerSettings
{
    public AbilityManagerSettings(
        float comboTimeout,
        float feedbackTimeout,
        float cooldownDuringCombo,
        float cooldownAfterCombo,
        bool rollResetsCombo
    )
    {
        ComboTimeout = comboTimeout;
        FeedbackTimeout = feedbackTimeout;
        CooldownDuringCombo = cooldownDuringCombo;
        CooldownAfterCombo = cooldownAfterCombo;
        RollResetsCombo = rollResetsCombo;
    }

    public float ComboTimeout { get; }
    public float FeedbackTimeout { get; }
    public float CooldownDuringCombo { get; }
    public float CooldownAfterCombo { get; }
    public bool RollResetsCombo { get; }
}