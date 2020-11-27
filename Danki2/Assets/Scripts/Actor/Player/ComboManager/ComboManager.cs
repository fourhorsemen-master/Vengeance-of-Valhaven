using System;
using System.Linq;

public class ComboManager
{
	private ObservableWorkflow<ComboState> workflow;

	public ComboManager(Player player, Subject updateSubject, float longCooldown, float shortCooldown, float comboTimeout, bool rollResetsCombo)
	{
		workflow = new ObservableWorkflow<ComboState>(ComboState.ReadyAtRoot)
			.WithProcessor(ComboState.ReadyAtRoot, new ReadyAtRootProcessor(player))
			.WithProcessor(ComboState.ReadyInCombo, new ReadyInComboProcessor(player, comboTimeout))
			.WithProcessor(ComboState.ChannelingLeft, new ChannelProcessor(player, Direction.Left))
			.WithProcessor(ComboState.ChannelingRight, new ChannelProcessor(player, Direction.Right))
			.WithProcessor(ComboState.AwaitingFeedback, new AwaitFeedbackProcessor(player))
			.WithProcessor(ComboState.CompleteCombo, new CompleteComboProcessor(player))
			.WithProcessor(ComboState.FailCombo, new FailComboProcessor(player))
			.WithProcessor(ComboState.ContinueCombo, new PassthroughProcessor<ComboState>(ComboState.ShortCooldown))
			.WithProcessor(ComboState.Whiff, new FailComboProcessor(player))
			.WithProcessor(ComboState.LongCooldown, new TimeElapsedProcessor<ComboState>(ComboState.ReadyAtRoot, longCooldown))
			.WithProcessor(ComboState.ShortCooldown, new TimeElapsedProcessor<ComboState>(ComboState.ReadyInCombo, shortCooldown));

		ForceTransitionOnEvent(player.HealthManager.DamageSubject, ComboState.Whiff);

		ForceTransitionOnEvent(player.AbilityTree.ChangeSubject, ComboState.Whiff, ComboState.ReadyAtRoot, ComboState.LongCooldown);

		if (rollResetsCombo)
		{
			ForceTransitionOnEvent(player.RollSubject, ComboState.Whiff);
		}

		workflow.Enter();

		updateSubject.Subscribe(workflow.Update);
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
