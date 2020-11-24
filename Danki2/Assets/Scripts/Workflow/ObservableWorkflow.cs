using System;

public class ObservableWorkflow<TState> where TState : Enum
{
	private TState currentState;

    private EnumDictionary<TState, Processor<TState>> processors;

    private IObservable<TState> OnEnterStateSubject = new Subject<TState>();
    private IObservable<TState> OnExitStateSubject = new Subject<TState>();

    public ObservableWorkflow(TState initialState)
	{
		currentState = initialState;
        processors = new EnumDictionary<TState, Processor<TState>>(() => new NoOpProcessor<TState>());
    }

    public void Enter() => processors[currentState].Enter();

    public void Exit() => processors[currentState].Exit();

    public void Update() {
        bool processComplete = processors[currentState].TryCompleteProcess(out TState nextState);

        if (!processComplete) return;
        
        if (nextState.Equals(currentState)) return;

        processors[currentState].Exit();
        OnExitStateSubject.Next(currentState);

        processors[nextState].Enter();
        OnEnterStateSubject.Next(nextState);

        currentState = nextState;
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