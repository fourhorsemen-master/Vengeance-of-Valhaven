using System;
using System.Linq;

public class ComboManager
{
	private readonly ObservableWorkflow<ComboState> workflow;

	public ComboManager(Player player, Subject updateSubject)
	{
		workflow = new ObservableWorkflow<ComboState>(ComboState.ReadyAtRoot, updateSubject)
			.WithProcessor(ComboState.ReadyAtRoot, new ReadyAtRootProcessor(player))
			.WithProcessor(ComboState.ReadyInCombo, new ReadyInComboProcessor(player))
			.WithProcessor(ComboState.CastLeft, new CastProcessor(player, Direction.Left))
			.WithProcessor(ComboState.CastRight, new CastProcessor(player, Direction.Right))
			.WithProcessor(ComboState.Casting, new AwaitAbilityCompletionProcessor(player, player.AbilityAnimationListener))
			.WithProcessor(ComboState.Interrupted, new EndComboProcessor(player))
			.WithProcessor(ComboState.Timeout, new EndComboProcessor(player))
			.WithProcessor(ComboState.CompleteCombo, new EndComboProcessor(player));

		player.InterruptSubject.Subscribe(
			() => workflow.ForceTransition(ComboState.Interrupted)
		);

		ForceTransitionOnEvent(player.AbilityTree.ChangeSubject, ComboState.Interrupted, ComboState.ReadyAtRoot);
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
