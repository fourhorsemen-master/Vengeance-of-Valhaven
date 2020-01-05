using UnityEngine;

public class Wolf : Enemy
{
    private AI<Wolf> _ai;

    protected override void Awake()
    {
        base.Awake();

        _ai = new AI<Wolf>(
            this,
            (a, b) => new Agenda(),
            new Personality<Wolf>()
        );
    }

    public override AI AI => _ai;

    protected override void OnDeath()
    {
        Debug.Log("Wolf died");
    }
}
