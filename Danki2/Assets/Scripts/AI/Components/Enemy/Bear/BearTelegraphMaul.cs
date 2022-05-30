using UnityEngine;

public class BearTelegraphMaul : TelegraphAttack
{
	private readonly Bear bear;

	public BearTelegraphMaul(Bear bear)
		: base(bear, Color.red, true)
	{
		this.bear = bear;
	}

	public override void Enter()
	{
		bear.PlayBiteWindupAnimation();
		base.Enter();
	}
}