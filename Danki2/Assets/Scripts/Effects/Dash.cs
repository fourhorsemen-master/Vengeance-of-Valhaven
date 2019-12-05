public class Dash : Effect
{
    private readonly float _speedMultiplier;
    private readonly float _slowDuration;
    private readonly float _slowMultiplier;

    public Dash(
        float duration,
        float speedMultiplier,
        float slowDuration,
        float slowMultiplier
    )
        : base(duration)
    {
        _speedMultiplier = speedMultiplier;
        _slowDuration = slowDuration;
        _slowMultiplier = slowMultiplier;
    }

    protected override void FinishAction(Actor actor)
    {
        actor.gameObject.GetComponent<EffectTracker>().AddEffect(
            new Slow(_slowDuration, _slowMultiplier)
        );
    }

    protected override void UpdateAction(Actor actor, float deltaTime)
    {
        // TODO: Implement dash update.
    }
}
