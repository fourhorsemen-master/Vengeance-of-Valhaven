public class WraithCastGuidedOrb : IStateMachineComponent
{
    private readonly Wraith wraith;
    private readonly Actor target;

    public WraithCastGuidedOrb(Wraith wraith, Actor target)
    {
        this.wraith = wraith;
        this.target = target;
    }

    public void Enter() => wraith.GuidedOrb(target);
    public void Exit() {}
    public void Update() {}
}
