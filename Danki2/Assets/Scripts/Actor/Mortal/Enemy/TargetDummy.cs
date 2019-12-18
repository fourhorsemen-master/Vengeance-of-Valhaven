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
            { AIAction.Advance, new FollowPlayerAtDistance<TargetDummy>() }
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
        // TODO: Implement TargetDummy death.
        Debug.Log("The target dummy died.");
    }
}