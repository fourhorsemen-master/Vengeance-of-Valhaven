public class CurrencyManager
{
    public int CurrencyAmount { get; private set; }

    public CurrencyManager()
    {
        CurrencyAmount = PersistenceManager.Instance.SaveData.CurrencyAmount;
    }

    public void AddCurrency(int amount) => CurrencyAmount += amount;
}
