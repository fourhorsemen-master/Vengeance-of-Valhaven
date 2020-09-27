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
        .WithTransition(WolfAiState.Attack, WolfAiState.Defend, new TimePeriodTrigger(1))
        .WithTransition(WolfAiState.Defend, WolfAiState.Attack, new TimePeriodTrigger(2));
}

public class TimePeriodTrigger : IAiTrigger
{
    private readonly float timePeriod;
    private float startTime;

    public TimePeriodTrigger(float timePeriod)
    {
        this.timePeriod = timePeriod;
    }

    public void Initialise()
    {
        startTime = Time.time;
    }

    public bool Triggers()
    {
        return Time.time - startTime >= timePeriod;
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
