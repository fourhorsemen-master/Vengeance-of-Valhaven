public static class GreedHandler
{
    private const int CurrencyGain = 100;

    public static void OnRuneAdded()
    {
        ActorCache.Instance.Player.CurrencyManager.AddCurrency(CurrencyGain);
    }
}
