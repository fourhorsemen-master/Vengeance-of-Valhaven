public class EntCastSpine : IStateMachineComponent
{
    private readonly Ent ent;
    private readonly Actor target;

    public EntCastSpine(Ent ent, Actor target)
    {
        this.ent = ent;
        this.target = target;
    }

    public void Enter() => ent.Spine(target);
    public void Exit() { }
    public void Update() { }
}
