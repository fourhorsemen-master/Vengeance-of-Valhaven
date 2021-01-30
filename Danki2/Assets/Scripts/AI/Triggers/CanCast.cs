internal class CanCast : StateMachineTrigger
{
    private Actor actor;

    public CanCast(Actor actor)
    {
        this.actor = actor;
    }

    public override void Activate() { }

    public override void Deactivate() { }

    public override bool Triggers() => actor.InstantCastService.CanCast;
}