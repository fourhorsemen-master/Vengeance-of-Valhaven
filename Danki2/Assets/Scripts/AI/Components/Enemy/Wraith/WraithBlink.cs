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
        wraith.transform.position = new Vector3(
            5,
            0,
            5
        );
    }

    public void Exit() {}
    public void Update() {}
}
