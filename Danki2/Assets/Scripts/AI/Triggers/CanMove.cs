public class CanMove : StateMachineTrigger
{
    private Actor actor;

    public CanMove(Actor actor)
    {
        this.actor = actor;
    }

    public override void Activate() { }

    public override void Deactivate() { }

    public override bool Triggers() => actor.MovementManager.CanMove;
}