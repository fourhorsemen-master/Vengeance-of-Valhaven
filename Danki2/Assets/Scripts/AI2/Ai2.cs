using UnityEngine;

public abstract class Ai2 : MonoBehaviour
{
    protected abstract IAiComponent AiComponent { get; }

    private void Start()
    {
        AiComponent.Enter();
    }

    private void Update()
    {
        AiComponent.Update();
    }
}
