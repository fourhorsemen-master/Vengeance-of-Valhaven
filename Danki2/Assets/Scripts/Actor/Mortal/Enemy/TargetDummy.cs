using System.Collections.Generic;
using UnityEngine;

public class TargetDummy : Enemy
{
    private AI<TargetDummy> _ai;

    protected override void Awake()
    {
        base.Awake();

        IPlanner<TargetDummy> planner = new AlwaysAdvance<TargetDummy>();
        Personality<TargetDummy> personality = new Personality<TargetDummy>
        {
            { AIAction.Advance, new FollowPlayerAtDistance<TargetDummy>(new float[] { 5f }) }
        };

        _ai = new AI<TargetDummy>(
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