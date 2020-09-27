using UnityEngine;

public abstract class Ai2 : MonoBehaviour
{
    [SerializeField]
    protected Actor actor = null;
    
    private IAiComponent aiComponent;

    private void Start()
    {
        actor.DeathSubject.Subscribe(OnDeath);
        aiComponent = GenerateAiComponent();
        aiComponent.Enter();
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

    protected abstract IAiComponent GenerateAiComponent();
}
