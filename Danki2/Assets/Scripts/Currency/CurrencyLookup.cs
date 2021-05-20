using System;
using UnityEngine;

[Serializable]
public class EnemyCurrencyValueLookup : SerializableEnumDictionary<ActorType, int>
{
    public EnemyCurrencyValueLookup(int defaultValue) : base(defaultValue) {}
    public EnemyCurrencyValueLookup(Func<int> defaultValueProvider) : base(defaultValueProvider) {}
}

public class CurrencyLookup : Singleton<CurrencyLookup>
{
    [SerializeField] private EnemyCurrencyValueLookup enemyCurrencyValueLookup = new EnemyCurrencyValueLookup(0);
    public EnemyCurrencyValueLookup EnemyCurrencyValueLookup { get => enemyCurrencyValueLookup; set => enemyCurrencyValueLookup = value; }
}
