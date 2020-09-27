using UnityEngine;

public abstract class Ai2 : MonoBehaviour
{
    private IAiComponent aiComponent;

    protected abstract Actor Actor { get; }

    private void Start()
    {
        Actor.DeathSubject.Subscribe(Die);

        aiComponent = GenerateAiComponent();
        aiComponent.Enter();
    }

    private void Update()
    {
        aiComponent.Update();
    }

    private void Die()
    {
        aiComponent.Exit();
        enabled = false;
    }

    protected abstract IAiComponent GenerateAiComponent();
}
