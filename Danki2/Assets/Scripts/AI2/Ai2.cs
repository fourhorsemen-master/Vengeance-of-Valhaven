using UnityEngine;

public abstract class Ai2 : MonoBehaviour
{
    private IAiComponent aiComponent;

    private void Start()
    {
        aiComponent = GenerateAiComponent();
        aiComponent.Enter();
    }

    private void Update()
    {
        aiComponent.Update();
    }

    protected abstract IAiComponent GenerateAiComponent();
}
