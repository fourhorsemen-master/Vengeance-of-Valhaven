using System;
using System.Linq;

public class ComboManager
{
	private readonly ObservableWorkflow<ComboState> workflow;

	public ComboManager(Player player, Subject updateSubject, bool rollResetsCombo, bool continousCombo)
	{
		workflow = new ObservableWorkflow<ComboState>(ComboState.ReadyAtRoot, updateSubject)
			.WithProcessor(ComboState.ReadyAtRoot, new ReadyAtRootProcessor(player))
			.WithProcessor(ComboState.ReadyInCombo, new ReadyInComboProcessor(player, player.ComboTimeout))
			.WithProcessor(ComboState.CastLeft, new CastProcessor(player, Direction.Left))
			.WithProcessor(ComboState.CastRight, new CastProcessor(player, Direction.Right))
			.WithProcessor(ComboState.Channeling, new ChannelProcessor(player))
			.WithProcessor(ComboState.FinishAbility, new FinishAbilityProcessor(player))
			.WithProcessor(ComboState.ContinueCombo, new PassthroughProcessor<ComboState>(ComboState.ShortCooldown))
			.WithProcessor(ComboState.ShortCooldown, new ShortCooldownProcessor(player, player.ShortCooldown, continousCombo))
			.WithProcessor(ComboState.CompleteCombo, new CompleteComboProcessor(player))
			.WithProcessor(ComboState.LongCooldown, new TimeElapsedProcessor<ComboState>(ComboState.ReadyAtRoot, player.LongCooldown))
			.WithProcessor(ComboState.Whiff, new FailComboProcessor(player));

		player.InterruptionManager.Register(
			InterruptionType.Soft,
			() => workflow.ForceTransition(ComboState.Whiff),
			InterruptibleFeature.Repeat
		);

		ForceTransitionOnEvent(player.AbilityTree.ChangeSubject, ComboState.Whiff, ComboState.ReadyAtRoot, ComboState.LongCooldown);

		if (rollResetsCombo)
		{
			ForceTransitionOnEvent(player.RollSubject, ComboState.Whiff);
		}
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
