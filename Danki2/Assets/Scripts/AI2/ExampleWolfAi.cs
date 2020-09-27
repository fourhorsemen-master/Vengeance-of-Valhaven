using UnityEngine;

public enum WolfAiState
{
    Attack,
    Defend
}

public class ExampleWolfAi : Ai2
{
    [SerializeField]
    private Actor actor;

    protected override IAiComponent GenerateAiComponent()
    {
        return new AiFiniteStateMachine<WolfAiState>(WolfAiState.Attack)
            .WithState(WolfAiState.Attack, new ExampleAttack())
            .WithState(WolfAiState.Defend, new ExampleDefend())
            .WithTransition(WolfAiState.Attack, WolfAiState.Defend, new HealthLostTrigger(actor, 5))
            .WithTransition(WolfAiState.Defend, WolfAiState.Attack, new TimePeriodTrigger(3f));
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

public class ExampleAttack : IAiComponent
{
    public void Enter()
    {
        Debug.Log("Starting Attack");
    }

    public void Update()
    {
        Debug.Log("Attacking");
    }
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
}
