public class CurrencyManager
{
    public int CurrencyAmount { get; private set; }

    public CurrencyManager()
    {
        CurrencyAmount = PersistenceManager.Instance.SaveData.CurrencyAmount;
    }
}
