using System;

public class ObservableWorkflow<TState> where TState : Enum
{
	public TState CurrentState { get; private set; }

    private EnumDictionary<TState, Processor<TState>> processors;

    private IObservable<TState> OnEnterStateSubject = new Subject<TState>();
    private IObservable<TState> OnExitStateSubject = new Subject<TState>();

    public ObservableWorkflow(TState initialState)
	{
		CurrentState = initialState;
        processors = new EnumDictionary<TState, Processor<TState>>(() => new NoOpProcessor<TState>());
    }

    public void Enter() => processors[CurrentState].Enter();

    public void Exit() => processors[CurrentState].Exit();

    public void Update() {
        bool processComplete = processors[CurrentState].TryCompleteProcess(out TState nextState);

        if (processComplete) Transition(nextState);
    }

    public void ForceTransition(TState toState) => Transition(toState);

    private void Transition(TState toState)
    {
        if (toState.Equals(CurrentState)) return;

        processors[CurrentState].Exit();
        OnExitStateSubject.Next(CurrentState);

        processors[toState].Enter();
        OnEnterStateSubject.Next(toState);

        CurrentState = toState;

        Update();
    }

    public ObservableWorkflow<TState> WithProcessor(TState state, Processor<TState> processor)
	{
		processors[state] = processor;

		return this;
	}

    public void SubscribeToStateEntry(TState state, Action action)
    {
        OnEnterStateSubject.Subscribe(newState =>
        {
            if (newState.Equals(state)) action();
        });
    }

    public void SubscribeToStateExit(TState state, Action action)
    {
        OnExitStateSubject.Subscribe(newState =>
        {
            if (newState.Equals(state)) action();
        });
    }
}