using UnityEngine;

public class BearTelegraphCharge : TelegraphAttack
{
	private readonly Bear bear;
	private readonly Actor target;

	public BearTelegraphCharge(Bear bear, Actor target)
		: base(bear, Color.red, true)
	{
		this.bear = bear;
		this.target = target;
	}

	public override void Enter()
	{
		bear.PlayBiteWindupAnimation();
		bear.MovementManager.SetRotationTarget(target.transform, null);
		base.Enter();
	}

	public override void Exit()
	{
		bear.MovementManager.SetRotationTarget(null, null);
		base.Exit();
	}
}