using UnityEngine;

public abstract class StateMachineMonoBehaviour : MonoBehaviour
{
    protected StateMachineComponent stateMachineComponent;

    protected virtual void Start()
    {
        stateMachineComponent = BuildStateMachineComponent();
        stateMachineComponent.Enter();
    }

    protected void Update()
    {
        stateMachineComponent.Update();
    }

    protected abstract StateMachineComponent BuildStateMachineComponent();
}
