using UnityEngine;

public class EntAi : Ai
{
    [SerializeField] private Ent ent = null;

    protected override Actor Actor => ent;

    protected override IStateMachineComponent BuildStateMachineComponent()
    {
        throw new System.NotImplementedException();
    }
}
