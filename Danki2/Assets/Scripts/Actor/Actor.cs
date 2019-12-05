using UnityEngine;

public abstract class Actor : MonoBehaviour
{
    private StatsManager _statsManager;
    private EffectTracker _effectTracker;

    protected virtual void Start()
    {
        _statsManager = gameObject.GetComponent<StatsManager>();
        _effectTracker = gameObject.GetComponent<EffectTracker>();
    }

    protected virtual void Update()
    {
        _statsManager.Rebase();
        _effectTracker.Process();
        this.Act();
    }

    protected abstract void Act();
}
