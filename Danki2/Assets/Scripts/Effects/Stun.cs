public class Stun : Effect
{
    private readonly float duration;

    public Stun(float duration)
    {
        this.duration = duration;
    }

    public override void Start(Actor actor)
    {
        actor.MovementManager.Stun(this.duration);
        actor.InterruptionManager.Interrupt(InterruptionType.Hard);
    }
}
