public class PlayerSettings
{
    public PlayerSettings() { }

    public PlayerSettings(
        float cooldownDuringCombo,
        float cooldownAfterCombo,
        float comboTimeout,
        bool rollResetsCombo,
        float totalRollCooldown,
        float rollDuration,
        float rollSpeedMultiplier
    )
    {
        this.CooldownDuringCombo = cooldownDuringCombo;
        this.CooldownAfterCombo = cooldownAfterCombo;
        this.ComboTimeout = comboTimeout;
        this.RollResetsCombo = rollResetsCombo;
        this.TotalRollCooldown = totalRollCooldown;
        this.RollDuration = rollDuration;
        this.RollSpeedMultiplier = rollSpeedMultiplier;
    }

    public float CooldownDuringCombo { get; } = 0.5f;
    public float CooldownAfterCombo { get; } = 2.5f;
    public float ComboTimeout { get; } = 2.5f;
    public bool RollResetsCombo { get; } = true;

    public float TotalRollCooldown { get; } = 1f;
    public float RollDuration { get; } = 0.2f;
    public float RollSpeedMultiplier { get; } = 3f;
}
