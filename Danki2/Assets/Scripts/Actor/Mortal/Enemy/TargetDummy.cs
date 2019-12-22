using System.Collections.Generic;
using UnityEngine;

public class TargetDummy : Enemy
{
    private AI _ai;

    protected override void Awake()
    {
        base.Awake();

        IPlanner planner = new AlwaysAdvance();
        Personality personality = new Personality
        {
            { AIAction.Advance, new FollowPlayerAtDistance(new float[] { 5f }) }
        };

        _ai = new AI(
            this,
            planner,
            personality
        );
    }

    public override AI AI => _ai;

    protected override void OnDeath()
    {
        gameObject.GetComponent<Rigidbody>().velocity = Vector3.up * 5;
        transform.Rotate(Vector3.forward, 90f);
    }
}