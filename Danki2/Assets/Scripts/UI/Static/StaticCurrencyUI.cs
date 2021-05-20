using UnityEngine;
using UnityEngine.UI;

public class StaticCurrencyUI : StaticUI<StaticCurrencyUI>
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
