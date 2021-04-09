using UnityEngine;

public abstract class StateMachineMonoBehaviour : MonoBehaviour
{
    protected IStateMachineComponent StateMachineComponent { get; private set; }

    protected virtual void Start()
    {
        StateMachineComponent = BuildStateMachineComponent();
        StateMachineComponent.Enter();
    }

    private void Update()
    {
        StateMachineComponent.Update();
    }

    private void OnDestroy()
    {
        StateMachineComponent.Exit();
    }

    protected abstract IStateMachineComponent BuildStateMachineComponent();
}
