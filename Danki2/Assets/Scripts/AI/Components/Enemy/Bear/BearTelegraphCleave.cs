using UnityEngine;

public class BearTelegraphCleave : TelegraphAttack
{
    private readonly Bear bear;

    public BearTelegraphCleave(Bear bear)
        : base(bear, Color.yellow)
    {
        this.bear = bear;
    }

    public override void Enter()
    {
        bear.PlayCleaveAnimation();
        base.Enter();
    }
}