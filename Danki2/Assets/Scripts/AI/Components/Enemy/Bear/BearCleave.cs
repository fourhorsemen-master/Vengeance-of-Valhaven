public class BearCleave : IStateMachineComponent
{
    private readonly Bear bear;

    public BearCleave(Bear bear)
    {
        this.bear = bear;
    }

    public void Enter() => bear.Cleave();

    public void Exit() { }
    public void Update() { }
}
