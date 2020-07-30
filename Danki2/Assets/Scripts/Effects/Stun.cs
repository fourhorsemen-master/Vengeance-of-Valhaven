public class Stun : Effect
{
    public override bool Stuns => true;

    public override void Start(Actor actor)
    {
        actor.InterruptionManager.Interrupt(InterruptionType.Hard);
    }
}
