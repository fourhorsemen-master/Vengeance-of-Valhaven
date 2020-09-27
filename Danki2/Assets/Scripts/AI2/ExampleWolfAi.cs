using UnityEngine;

public class ExampleWolfAi : Ai2
{
    protected override IAiComponent GenerateAiComponent()
    {
        IAiComponent attackStateMachine = new AiFiniteStateMachine<WolfAttackAiState>(WolfAttackAiState.Attack1)
            .WithState(WolfAttackAiState.Attack1, new ExampleAttack1())
            .WithState(WolfAttackAiState.Attack2, new ExampleAttack2())
            .WithTransition(WolfAttackAiState.Attack1, WolfAttackAiState.Attack2, new TimePeriodTrigger(1))
            .WithTransition(WolfAttackAiState.Attack2, WolfAttackAiState.Attack1, new TimePeriodTrigger(1));

        return new AiFiniteStateMachine<WolfAiState>(WolfAiState.Attack)
            .WithState(WolfAiState.Attack, attackStateMachine)
            .WithState(WolfAiState.Defend, new ExampleDefend())
            .WithTransition(WolfAiState.Attack, WolfAiState.Defend, new HealthLostTrigger(actor, 5), new TimePeriodTrigger(10))
            .WithTransition(WolfAiState.Defend, WolfAiState.Attack, new TimePeriodTrigger(4));
    }

    private enum WolfAiState
    {
        Attack,
        Defend
    }

    private enum WolfAttackAiState
    {
        Attack1,
        Attack2
    }
}

public class HealthLostTrigger : IAiTrigger
{
    private readonly Actor actor;
    private readonly int healthAmount;

    private int initialHealth;

    public HealthLostTrigger(Actor actor, int healthAmount)
    {
        this.actor = actor;
        this.healthAmount = healthAmount;
    }

    public void Initialise()
    {
        initialHealth = actor.HealthManager.Health;
    }

    public bool Triggers()
    {
        return actor.HealthManager.Health <= initialHealth - healthAmount;
    }
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

public class ExampleAttack1 : IAiComponent
{
    public void Enter()
    {
        Debug.Log("Starting Attack 1");
    }

    public void Update()
    {
        Debug.Log("Attacking with attack 1");
    }

    public void Exit() { }
}

public class ExampleAttack2 : IAiComponent
{
    public void Enter()
    {
        Debug.Log("Starting Attack 2");
    }

    public void Update()
    {
        Debug.Log("Attacking with attack 2");
    }

    public void Exit() { }
}

public class ExampleDefend : IAiComponent
{
    public void Enter()
    {
        Debug.Log("Starting Defend");
    }

    public void Update()
    {
        Debug.Log("Defending");
    }

    public void Exit() { }
}

