using UnityEngine;

public class BearTelegraphCharge : TelegraphAttack
{
	private readonly Bear bear;

	public BearTelegraphCharge(Bear bear)
		: base(bear, Color.red)
	{
		this.bear = bear;
	}

	public override void Enter()
	{
		bear.PlayBiteWindupAnimation();
		base.Enter();
	}
}