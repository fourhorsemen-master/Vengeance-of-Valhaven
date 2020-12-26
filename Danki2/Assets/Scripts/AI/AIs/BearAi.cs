using UnityEngine;

public class BearAi : Ai
{
    [SerializeField] private Bear bear = null;

    protected override Actor Actor => bear;

    protected override IStateMachineComponent BuildStateMachineComponent()
    {
        return new NoOpComponent();
    }
}
