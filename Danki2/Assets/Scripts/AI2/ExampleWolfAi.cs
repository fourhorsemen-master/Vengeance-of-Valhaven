using UnityEngine;

public enum WolfAiState
{
    Attack,
    Defend
}

public class ExampleWolfAi : Ai2
{
    protected override IAiComponent AiComponent { get; } = new AiFiniteStateMachine<WolfAiState>(WolfAiState.Attack)
        .WithState(WolfAiState.Attack, new ExampleAttack())
        .WithState(WolfAiState.Defend, new ExampleDefend())
        .WithTransition(WolfAiState.Attack, WolfAiState.Defend, new RandomTrigger())
        .WithTransition(WolfAiState.Defend, WolfAiState.Attack, new RandomTrigger());
}

public class RandomTrigger : IAiTrigger
{
    public bool Triggers()
    {
        return Random.value < 0.001;
    }
}

public class ExampleAttack : IAiComponent
{
    public void Enter()
    {
        Debug.Log("Start Attack");
    }

    public void Update()
    {
    }
}

public class ExampleDefend : IAiComponent
{
    public void Enter()
    {
        Debug.Log("Start Defend");
    }

    public void Update()
    {
    }
}
