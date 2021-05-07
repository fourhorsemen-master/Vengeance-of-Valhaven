public class WraithTelegraphBlink : IStateMachineComponent
{
    private readonly Wraith wraith;

    public WraithTelegraphBlink(Wraith wraith)
    {
        this.wraith = wraith;
    }

    public void Enter() => wraith.TelegraphBlink();
    public void Exit() {}
    public void Update() {}
}
