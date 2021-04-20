using UnityEngine;

public class WraithBlink : IStateMachineComponent
{
    private readonly Wraith wraith;

    public WraithBlink(Wraith wraith)
    {
        this.wraith = wraith;
    }

    public void Enter()
    {
        wraith.InstantCastService.TryCast(
            AbilityReference.Blink,
            wraith.transform.position + Vector3.forward,
            wraith.transform.position + Vector3.forward
        );
    }

    public void Exit() {}
    public void Update() {}
}
