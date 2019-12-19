using UnityEngine;

public class TargetDummy : Enemy
{
    private AI<TargetDummy> _ai;

    protected override void Awake()
    {
        base.Awake();

        _ai = new AI<TargetDummy>(
            this,
            (a, b) => new Agenda(),
            new Personality<TargetDummy>()
        );
    }

    public override AI AI => _ai;

    protected override void OnDeath()
    {
        gameObject.GetComponent<Rigidbody>().velocity = Vector3.up * 5;
        transform.Rotate(Vector3.forward, 90f);
    }
}