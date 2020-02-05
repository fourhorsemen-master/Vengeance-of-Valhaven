using UnityEngine;

public class Whirlwind : Channel
{
    private static readonly float spinRange = 2;
    private static readonly float spinDpsMultiplier = 0.5f;
    private static readonly float slowMultiplier = 0.5f;
    private static readonly float finishRange = 3f;
    private static readonly float finishDamageMultiplier = 2;

    public Whirlwind(AbilityContext context) : base(context) { }

    public override float Duration => 3f;

    public override void Start()
    {
        AOE(spinRange, spinDpsMultiplier * Time.deltaTime);
    }

    public override void Continue()
    {
        AOE(spinRange, spinDpsMultiplier * Time.deltaTime);
    }

    public override void End()
    {
        AOE(finishRange, finishDamageMultiplier);
    }

    private void AOE(float size, float damageMultiplier)
    {

    }
}
