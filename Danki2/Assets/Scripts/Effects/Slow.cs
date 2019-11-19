public class Slow : Effect
{
    private readonly float _slowMultiplier;

    public Slow(float duration, float slowMultiplier) : base(duration)
    {
        _slowMultiplier = slowMultiplier;
    }

    protected override void FinishAction(Actor actor)
    {
    }

    protected override void UpdateAction(Actor actor, float deltaTime)
    {
        // TODO: Implement slow update
    }
}
