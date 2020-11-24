using System;

public class ComboManager
{
	private readonly Player player;
	private readonly Subject updateSubject;
	private ObservableWorkflow<ComboState> workflow;

	public ComboManager(Player player, Subject updateSubject)
	{
		this.player = player;
		this.updateSubject = updateSubject;

		workflow = new ObservableWorkflow<ComboState>(ComboState.ReadyAtRoot)
			.WithProcessor(ComboState.ReadyAtRoot, new NoOpProcessor<ComboState>());

		workflow.Enter();

		updateSubject.Subscribe(workflow.Update);
    }

	public void SubscribeToStateEntry(ComboState state, Action action) => workflow.SubscribeToStateEntry(state, action);

    public void SubscribeToStateExit(ComboState state, Action action) => workflow.SubscribeToStateExit(state, action);
}
