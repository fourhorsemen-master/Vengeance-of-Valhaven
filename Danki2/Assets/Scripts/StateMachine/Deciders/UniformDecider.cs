using System;

public class UniformDecider<TState> : IStateMachineDecider<TState> where TState : Enum
{
    private readonly TState[] options;

    public UniformDecider(params TState[] options)
    {
        this.options = options;
    }

    public TState Decide() => RandomUtils.Choice(options);
}
