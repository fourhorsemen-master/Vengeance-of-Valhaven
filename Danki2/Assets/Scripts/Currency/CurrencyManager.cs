public class CurrencyManager
{
    public BehaviourSubject<int> CurrencySubject { get; }
    public int CurrencyAmount => CurrencySubject.Value;

    public CurrencyManager()
    {
        CurrencySubject = new BehaviourSubject<int>(PersistenceManager.Instance.SaveData.CurrencyAmount);
    }

    public void AddCurrency(int amount) => CurrencySubject.Next(CurrencyAmount + amount);
}
