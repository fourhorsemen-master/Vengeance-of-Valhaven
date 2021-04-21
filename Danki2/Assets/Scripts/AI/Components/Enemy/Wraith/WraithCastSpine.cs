public class WraithCastSpine : IStateMachineComponent
{
    private readonly Wraith wraith;
    private readonly Actor target;

    public WraithCastSpine(Wraith wraith, Actor target)
    {
        this.wraith = wraith;
        this.target = target;
    }

    public void Enter() => wraith.Spine(target);
    public void Exit() {}
    public void Update() {}
}
