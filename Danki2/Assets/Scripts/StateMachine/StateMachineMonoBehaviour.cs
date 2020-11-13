using UnityEngine;

public abstract class StateMachineMonoBehaviour : MonoBehaviour
{
    protected StateMachineComponent StateMachineComponent { get; private set; }

    protected virtual void Start()
    {
        StateMachineComponent = BuildStateMachineComponent();
        StateMachineComponent.Enter();
    }

    protected void Update()
    {
        StateMachineComponent.Update();
    }

    protected abstract StateMachineComponent BuildStateMachineComponent();
}
