using UnityEngine;

public abstract class StateMachineMonoBehaviour : MonoBehaviour
{
    protected IStateMachineComponent StateMachineComponent { get; private set; }

    protected virtual void Start()
    {
        StateMachineComponent = BuildStateMachineComponent();
        StateMachineComponent.Enter();
    }

    protected void Update()
    {
        StateMachineComponent.Update();
    }

    protected abstract IStateMachineComponent BuildStateMachineComponent();
}
