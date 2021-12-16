public static class PassiveSlowHandler
{
    private const float SpeedMultiplier = 0.5f;

    public static float ProcessSpeed(float value)
    {
        return ActorCache.Instance.Player.EffectManager.HasPassiveEffect(PassiveEffect.Slow)
            ? value * SpeedMultiplier
            : value;
    }
}
