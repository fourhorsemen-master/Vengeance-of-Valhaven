public class BearSwipe : IStateMachineComponent
{
    private readonly Bear bear;

    public BearSwipe(Bear bear)
    {
        this.bear = bear;
    }

    public void Enter() => bear.Swipe();

    public void Exit() { }
    public void Update() { }
}
