class DOT : Effect
{
    private readonly int damagePerTick;
    private readonly float tickInterval;

    private Repeater repeater;

    public DOT(int damagePerTick, float tickInterval)
    {
        this.damagePerTick = damagePerTick;
        this.tickInterval = tickInterval;
    }

    public override void Start(Actor actor)
    {
        repeater = new Repeater(
            tickInterval,
            () => actor.HealthManager.TickDamage(-damagePerTick),
            tickInterval
        );
    }

    public override void Update(Actor actor)
    {
        repeater.Update();
    }
}
