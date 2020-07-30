public class Stun : Effect
{
    public override bool Stuns => true;

    //private readonly float duration;

    //public Stun(float duration)
    //{
    //    this.duration = duration;
    //}

    public override void Start(Actor actor)
    {
        actor.InterruptionManager.Interrupt(InterruptionType.Hard);
    }
}
