using System;
using System.Linq;

public class ComboManager
{
	private readonly ObservableWorkflow<ComboState> workflow;

	public ComboManager(Player player, Subject updateSubject)
	{
		workflow = new ObservableWorkflow<ComboState>(ComboState.ReadyAtRoot, updateSubject)
			.WithProcessor(ComboState.ReadyAtRoot, new ReadyAtRootProcessor(player))
			.WithProcessor(ComboState.ReadyInCombo, new ReadyInComboProcessor(player, player.ComboTimeout))
			.WithProcessor(ComboState.CastLeft, new CastProcessor(player, Direction.Left))
			.WithProcessor(ComboState.CastRight, new CastProcessor(player, Direction.Right))
			.WithProcessor(ComboState.ShortCooldown, new AwaitAbilityCopmletionProcessor(player, player.AbilityAnimationListener))
			.WithProcessor(ComboState.CompleteCombo, new CompleteComboProcessor(player))
			.WithProcessor(ComboState.Interrupted, new InterruptedProcessor(player))
			.WithProcessor(ComboState.LongCooldown, new TimeElapsedProcessor<ComboState>(ComboState.ReadyAtRoot, player.LongCooldown))
			.WithProcessor(ComboState.Timeout, new TimeoutProcessor(player))
			.WithProcessor(ComboState.TimeoutCooldown, new TimeElapsedProcessor<ComboState>(ComboState.ReadyAtRoot, player.LongCooldown - player.ComboTimeout));

		player.InterruptionManager.Register(
			InterruptionType.Soft,
			() => workflow.ForceTransition(ComboState.Interrupted),
			InterruptibleFeature.Repeat
		);

		ForceTransitionOnEvent(player.AbilityTree.ChangeSubject, ComboState.Interrupted, ComboState.ReadyAtRoot, ComboState.LongCooldown);
	}

	public void SubscribeToStateEntry(ComboState state, Action action) => workflow.SubscribeToStateEntry(state, action);

    public void SubscribeToStateExit(ComboState state, Action action) => workflow.SubscribeToStateExit(state, action);

	private void ForceTransitionOnEvent(Subject subject, ComboState toState, params ComboState[] exceptions)
	{
		subject.Subscribe(() =>
		{
			if (exceptions.ToList().Contains(workflow.CurrentState)) return;

			workflow.ForceTransition(toState);
		});
	}
}
