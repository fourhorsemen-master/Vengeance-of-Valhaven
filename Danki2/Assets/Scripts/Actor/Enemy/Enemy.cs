using UnityEngine;

public abstract class Enemy : Actor
{
    public EnemyMovementManager MovementManager { get; private set; }

    public BehaviourSubject<bool> PlayerTargeted { get; } = new BehaviourSubject<bool>(false);

    public Color? CurrentTelegraph { get; private set; } = null;

    protected override Tag Tag => Tag.Enemy;

    protected override void Awake()
    {
        base.Awake();

        MovementManager = new EnemyMovementManager(this, updateSubject, navmeshAgent);
    }

    public void StartTelegraph(Color colour) => CurrentTelegraph = colour;

    public void StopTelegraph() => CurrentTelegraph = null;
}
