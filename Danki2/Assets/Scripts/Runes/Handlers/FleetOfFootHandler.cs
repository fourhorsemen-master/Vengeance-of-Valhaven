public static class FleetOfFootHandler
{
    private const float SpeedMultiplierBonus = 1.25f;

    public static float ProcessSpeed(float value)
    {
        return ActorCache.Instance.Player.RuneManager.HasRune(Rune.FleetOfFoot)
            ? value * SpeedMultiplierBonus
            : value;
    }
}
