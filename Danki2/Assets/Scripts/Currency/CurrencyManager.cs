public class CurrencyManager
{
    public BehaviourSubject<int> CurrencyChangedSubject { get; }
    public int CurrencyAmount => CurrencyChangedSubject.Value;

    public CurrencyManager()
    {
        CurrencyChangedSubject = new BehaviourSubject<int>(PersistenceManager.Instance.SaveData.CurrencyAmount);
    }

    public void AddCurrency(int amount) => CurrencyChangedSubject.Next(CurrencyAmount + amount);
}
