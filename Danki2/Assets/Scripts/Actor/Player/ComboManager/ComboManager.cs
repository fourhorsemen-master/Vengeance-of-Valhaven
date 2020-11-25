using System;

public class ComboManager
{
	private ObservableWorkflow<ComboState> workflow;

	private float longCooldown = 0f;
	private float shortCooldown = 0f;
	private float feedbackTimeout = 0f;
	private float comboTimeout = 0f;

	public bool? FeedbackSinceLastCast { get; private set; } = null;

	public ComboManager(Player player, Subject updateSubject)
	{
		workflow = new ObservableWorkflow<ComboState>(ComboState.ReadyAtRoot)
			.WithProcessor(ComboState.ReadyAtRoot, new ReadyAtRootProcessor(player))
			.WithProcessor(ComboState.ReadyInCombo, new ReadyInComboProcessor(player, comboTimeout))
			.WithProcessor(ComboState.CastLeft, new CastProcessor(player, f => FeedbackSinceLastCast = f Direction.Left))
			.WithProcessor(ComboState.CastRight, new CastProcessor(player, f => FeedbackSinceLastCast = f, Direction.Right))
			.WithProcessor(ComboState.ChannelingRight, new ChannelProcessor(player, Direction.Left))
			.WithProcessor(ComboState.ChannelingLeft, new ChannelProcessor(player, Direction.Right))
			.WithProcessor(ComboState.AwaitingFeedback, new AwaitFeedbackProcessor(player, feedbackTimeout))
			.WithProcessor(ComboState.CompleteCombo, new PassthroughProcessor<ComboState>(ComboState.LongCooldown))
			.WithProcessor(ComboState.FailCombo, new PassthroughProcessor<ComboState>(ComboState.LongCooldown))
			.WithProcessor(ComboState.ContinueCombo, new PassthroughProcessor<ComboState>(ComboState.ShortCooldown))
			.WithProcessor(ComboState.Whiff, new PassthroughProcessor<ComboState>(ComboState.LongCooldown))
			.WithProcessor(ComboState.LongCooldown, new TimeElapsedProcessor<ComboState>(ComboState.ReadyAtRoot, longCooldown))
			.WithProcessor(ComboState.ShortCooldown, new TimeElapsedProcessor<ComboState>(ComboState.ReadyInCombo, shortCooldown));

		workflow.Enter();

		updateSubject.Subscribe(workflow.Update);
    }

	public void SubscribeToStateEntry(ComboState state, Action action) => workflow.SubscribeToStateEntry(state, action);

    public void SubscribeToStateExit(ComboState state, Action action) => workflow.SubscribeToStateExit(state, action);
}
