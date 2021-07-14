public class WraithCastRapidSpine : IStateMachineComponent
{
    private readonly Wraith wraith;
    private readonly Actor target;

    public WraithCastRapidSpine(Wraith wraith, Actor target)
    {
        this.wraith = wraith;
        this.target = target;
    }

    public void Enter() => wraith.RapidSpine(target);
    public void Exit() { }
    public void Update() { }
}
