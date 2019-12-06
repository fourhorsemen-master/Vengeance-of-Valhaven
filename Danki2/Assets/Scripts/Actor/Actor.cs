using UnityEngine;

public abstract class Actor : MonoBehaviour
{
    protected StatsManager _statsManager;
    protected EffectTracker _effectTracker;
    protected MovementManager _movementManager;

    protected virtual void Start()
    {
        _statsManager = gameObject.GetComponent<StatsManager>();
        _effectTracker = gameObject.GetComponent<EffectTracker>();
        _movementManager = gameObject.GetComponent<MovementManager>();
    }

    protected virtual void Update()
    {
        _statsManager.Rebase();
        _effectTracker.Process();
        this.Act();
        _movementManager.ExecuteMovement();
    }

    protected abstract void Act();
}
