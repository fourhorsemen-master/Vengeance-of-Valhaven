using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Enemy : Actor
{
    private readonly List<float> currencyValueMultipliers = new List<float>();

    public EnemyMovementManager MovementManager { get; private set; }

    public BehaviourSubject<bool> PlayerTargeted { get; } = new BehaviourSubject<bool>(false);

    public Color? CurrentTelegraph { get; private set; } = null;

    protected override Tag Tag => Tag.Enemy;

    protected override void Awake()
    {
        base.Awake();

        MovementManager = new EnemyMovementManager(this, updateSubject, navmeshAgent);
    }

    public void StartTelegraph(Color colour) => CurrentTelegraph = colour;

    public void StopTelegraph() => CurrentTelegraph = null;

    public int GetCurrencyValue()
    {
        float baseValue = CurrencyLookup.Instance.EnemyCurrencyValueLookup[Type];

        float multiplier = currencyValueMultipliers.Aggregate((a, b) => a * b);

        return Mathf.FloorToInt(baseValue * multiplier);
    }

    public void AddCurrencyValueMultiplier(float multiplier)
    {
        currencyValueMultipliers.Add(multiplier);
    }
}
