using System;

public class CyclicalDecider<TState> : IStateMachineDecider<TState> where TState : Enum
{
    private readonly TState[] options;
    private int index = 0;

    public CyclicalDecider(params TState[] options)
    {
        this.options = options;
    }

    public TState Decide()
    {
        TState choice = options[index];
        index = (index + 1) % options.Length;
        return choice;
    }
}
