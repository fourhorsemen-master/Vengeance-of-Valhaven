using UnityEngine;

class DOT : Effect
{
    private readonly int[] damageArray;
    private readonly float tickInterval;

    private int damageIndex = 0;

    private Repeater repeater;

    public DOT(int totalDamage, float duration, float tickInterval = 1f)
    {
        this.tickInterval = Mathf.Min(tickInterval, duration);

        int baseTickDamage = Mathf.FloorToInt(totalDamage * this.tickInterval / duration);
        int numberOfTicks = Mathf.FloorToInt(duration / this.tickInterval);
        int remainderTickDamage = totalDamage - (baseTickDamage * numberOfTicks);

        this.damageArray = new int[numberOfTicks];

        for (int i = numberOfTicks - 1; i >= 0; i--)
        {
            damageArray[i] = remainderTickDamage > 0
                ? baseTickDamage + 1
                : baseTickDamage;
            remainderTickDamage--;
        }
    }

    public override void Start(Actor actor)
    {
        repeater = new Repeater(
            tickInterval,
            () =>
            {
                actor.HealthManager.TickDamage(damageArray[damageIndex]);
                damageIndex++;
            },
            tickInterval
        );
    }

    public override void Update(Actor actor)
    {
        repeater.Update();
    }
}
