using System;

public class RandomDecider<TState> : IStateMachineDecider<TState> where TState : Enum
{
    private readonly TState[] options;

    public RandomDecider(params TState[] options)
    {
        this.options = options;
    }

    public TState Decide()
    {
        return RandomUtils.Choice(options);
    }
}
