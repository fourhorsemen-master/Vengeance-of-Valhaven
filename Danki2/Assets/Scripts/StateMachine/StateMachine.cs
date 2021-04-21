using System;
using System.Collections.Generic;

public class StateMachine<TState> : IStateMachineComponent where TState : Enum
{
    private readonly EnumDictionary<TState, IStateMachineComponent> components =
        new EnumDictionary<TState, IStateMachineComponent>(() => new NoOpComponent());
    
    private readonly EnumDictionary<TState, EnumDictionary<TState, StateMachineTrigger>> localTriggers =
        new EnumDictionary<TState, EnumDictionary<TState, StateMachineTrigger>>(() =>
            new EnumDictionary<TState, StateMachineTrigger>(new NeverTrigger()));

    private readonly EnumDictionary<TState, StateMachineTrigger> globalTriggers =
        new EnumDictionary<TState, StateMachineTrigger>(new NeverTrigger());

    private readonly Dictionary<TState, IStateMachineDecider<TState>> deciders = new Dictionary<TState, IStateMachineDecider<TState>>();

    private readonly TState initialState;
    private TState currentState;

    public StateMachine(TState initialState)
    {
        this.initialState = initialState;
    }

    public StateMachine<TState> WithComponent(TState state, IStateMachineComponent component)
    {
        components[state] = component;
        return this;
    }

    public StateMachine<TState> WithTransition(TState from, TState to, StateMachineTrigger stateMachineTrigger = null)
    {
        localTriggers[from][to] = stateMachineTrigger ?? new AlwaysTrigger();
        return this;
    }

    public StateMachine<TState> WithGlobalTransition(TState to, StateMachineTrigger stateMachineTrigger)
    {
        globalTriggers[to] = stateMachineTrigger;
        return this;
    }

    public StateMachine<TState> WithDecisionState(TState fromState, IStateMachineDecider<TState> decider)
    {
        deciders[fromState] = decider;
        return this;
    }

    public void Enter()
    {
        currentState = initialState;
        ActivateTriggers();
        components[currentState].Enter();
    }

    public void Exit()
    {
        DeactivateTriggers();
        components[currentState].Exit();
    }

    public void Update()
    {
        components[currentState].Update();
        TryTransition();
    }

    private void TryTransition()
    {
        if (deciders.ContainsKey(currentState))
        {
            TState nextState = deciders[currentState].Decide();

            Transition(nextState);
            return;
        }

        foreach (KeyValuePair<TState, StateMachineTrigger> potentialTransition in localTriggers[currentState])
        {
            TState toState = potentialTransition.Key;
            StateMachineTrigger stateMachineTrigger = potentialTransition.Value;
            
            if (stateMachineTrigger.Triggers())
            {
                Transition(toState);
                return;
            }
        }
        
        foreach (KeyValuePair<TState, StateMachineTrigger> potentialTransition in globalTriggers)
        {
            TState toState = potentialTransition.Key;
            StateMachineTrigger stateMachineTrigger = potentialTransition.Value;

            if (toState.Equals(currentState))
            {
                continue;
            }

            if (stateMachineTrigger.Triggers())
            {
                Transition(toState);
                return;
            }
        }
    }

    private void Transition(TState toState)
    {
        DeactivateTriggers();
        components[currentState].Exit();

        currentState = toState;
        ActivateTriggers();
        components[currentState].Enter();
    }

    private void ActivateTriggers()
    {
        ForEachGlobalTrigger(t => t.Activate());
        ForEachLocalTrigger(t => t.Activate());
    }

    private void DeactivateTriggers()
    {
        ForEachGlobalTrigger(t => t.Deactivate());
        ForEachLocalTrigger(t => t.Deactivate());
    }

    private void ForEachGlobalTrigger(Action<StateMachineTrigger> action)
    {
        foreach (StateMachineTrigger trigger in globalTriggers.Values)
        {
            action(trigger);
        }
    }

    private void ForEachLocalTrigger(Action<StateMachineTrigger> action)
    {
        foreach (StateMachineTrigger trigger in localTriggers[currentState].Values)
        {
            action(trigger);
        }
    }
}
