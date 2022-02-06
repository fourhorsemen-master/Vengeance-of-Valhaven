using UnityEngine;

public class BearTelegraphSwipe : TelegraphAttack
{
    private readonly Bear bear;

    public BearTelegraphSwipe(Bear bear)
        : base(bear, Color.red)
    {
        this.bear = bear;
    }

    public override void Enter()
    {
        bear.PlaySwipeAnimation();
        base.Enter();
    }
}