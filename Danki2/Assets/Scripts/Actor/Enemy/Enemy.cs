using UnityEngine;

public abstract class Enemy : Actor
{
    public BehaviourSubject<bool> PlayerTargeted { get; } = new BehaviourSubject<bool>(false);

    public Color? CurrentTelegraph { get; private set; } = null;

    protected override Tag Tag => Tag.Enemy;

    public void StartTelegraph(Color colour) => CurrentTelegraph = colour;

    public void StopTelegraph() => CurrentTelegraph = null;
}
