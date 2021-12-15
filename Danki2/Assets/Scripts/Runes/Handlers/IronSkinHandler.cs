public static class IronSkinHandler
{
    private const int HealthBonus = 10;

    public static int ProcessMaxHealth(int value)
    {
        return ActorCache.Instance.Player.RuneManager.HasRune(Rune.IronSkin)
            ? value + HealthBonus
            : value;
    }
}
