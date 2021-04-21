public class WraithCastSwipe : IStateMachineComponent
{
    private readonly Wraith wraith;

    public WraithCastSwipe(Wraith wraith)
    {
        this.wraith = wraith;
    }

    public void Enter() => wraith.Swipe();
    public void Exit() {}
    public void Update() {}
}
