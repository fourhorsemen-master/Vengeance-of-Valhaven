using UnityEngine;

class DOT : Effect
{
    private readonly float damagePerTick;
    private readonly float tickInterval;

    private Repeater repeater;

    public DOT(float damagePerTick, float tickInterval)
    {
        this.damagePerTick = damagePerTick;
        this.tickInterval = tickInterval;
    }

    public override void Start(Actor actor)
    {
        repeater = new Repeater(tickInterval, () => actor.ModifyHealth(-damagePerTick), tickInterval);
    }

    public override void Update(Actor actor)
    {
        repeater.Update();
    }
}
