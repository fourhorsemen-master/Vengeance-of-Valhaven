public static class ActiveSlowHandler
{
    private const float SlowMultiplier = 0.5f;

    public static float ProcessSpeed(float value)
    {
        return ActorCache.Instance.Player.EffectManager.HasActiveEffect(ActiveEffect.Slow)
            ? value * SlowMultiplier
            : value;
    }
}
