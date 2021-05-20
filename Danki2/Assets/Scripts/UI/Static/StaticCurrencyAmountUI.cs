using UnityEngine;
using UnityEngine.UI;

public class StaticCurrencyAmountUI : StaticUI<StaticCurrencyAmountUI>
{
    [SerializeField] private Text text = null;

    protected override void Start()
    {
        base.Start();

        ActorCache.Instance.Player.CurrencyManager.CurrencyChangedSubject.Subscribe(amount =>
        {
            text.text = $"Currency: {amount.ToString()}";
        });
    }
}
