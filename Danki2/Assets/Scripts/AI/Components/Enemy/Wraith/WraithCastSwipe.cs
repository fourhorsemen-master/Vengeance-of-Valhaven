using UnityEngine;

public class WraithCastSwipe : IStateMachineComponent
{
    private readonly Wraith wraith;

    public WraithCastSwipe(Wraith wraith, Actor target)
    {
        this.wraith = wraith;
    }

    public void Enter() => Debug.Log("Casting swipe...");
    public void Exit() {}
    public void Update() {}
}
