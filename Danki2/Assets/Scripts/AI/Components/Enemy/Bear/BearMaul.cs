public class BearMaul : IStateMachineComponent
{
    private readonly Bear bear;

    public BearMaul(Bear bear)
    {
        this.bear = bear;
    }

    public void Enter() => bear.Maul();
    public void Exit() { }
    public void Update() { }
}
