using UnityEngine;

public abstract class Ai2 : MonoBehaviour
{
    protected abstract Actor Actor { get; }
    
    private IAiComponent aiComponent;

    private void Start()
    {
        aiComponent = BuildAiComponent();
        aiComponent.Enter();
        Actor.DeathSubject.Subscribe(OnDeath);
    }

    private void Update()
    {
        aiComponent.Update();
    }

    private void OnDeath()
    {
        aiComponent.Exit();
        enabled = false;
    }

    protected abstract IAiComponent BuildAiComponent();
}
