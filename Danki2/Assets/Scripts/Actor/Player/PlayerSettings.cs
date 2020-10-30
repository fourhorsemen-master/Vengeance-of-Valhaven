using System;

[Serializable]
public class PlayerSettings
{
    public PlayerSettings()
    {
    }

    public PlayerSettings(
        float cooldownDuringCombo,
        float cooldownAfterCombo,
        float comboTimeout,
        float feedbackTimeout,
        bool rollResetsCombo,
        float totalRollCooldown,
        float rollDuration,
        float rollSpeedMultiplier
    )
    {
        CooldownDuringCombo = cooldownDuringCombo;
        CooldownAfterCombo = cooldownAfterCombo;
        ComboTimeout = comboTimeout;
        FeedbackTimeout = feedbackTimeout;
        RollResetsCombo = rollResetsCombo;
        TotalRollCooldown = totalRollCooldown;
        RollDuration = rollDuration;
        RollSpeedMultiplier = rollSpeedMultiplier;
    }

    public float CooldownDuringCombo;
    public float CooldownAfterCombo;
    public float ComboTimeout;
    public float FeedbackTimeout;
    public bool RollResetsCombo;

    public float TotalRollCooldown;
    public float RollDuration;
    public float RollSpeedMultiplier;
}
