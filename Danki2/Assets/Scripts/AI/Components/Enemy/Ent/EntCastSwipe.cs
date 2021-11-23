public class EntCastSwipe : IStateMachineComponent
{
    private readonly Ent ent;

    public EntCastSwipe(Ent ent)
    {
        this.ent = ent;
    }

    public void Enter() => ent.Swipe();
    public void Exit() { }
    public void Update() { }
}
