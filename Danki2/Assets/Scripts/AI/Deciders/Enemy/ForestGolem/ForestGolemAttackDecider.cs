using UnityEngine;
using State = ForestGolemAi.State;

public class ForestGolemAttackDecider : IStateMachineDecider<State>
{
    private readonly ForestGolem forestGolem;
    private readonly float rootStormCooldown;
    private readonly float stompRange;
    
    private float lastRootStormTime = -1;

    public ForestGolemAttackDecider(ForestGolem forestGolem, float rootStormCooldown, float stompRange)
    {
        this.forestGolem = forestGolem;
        this.rootStormCooldown = rootStormCooldown;
        this.stompRange = stompRange;
    }

    public State Decide()
    {
        if (lastRootStormTime == -1) lastRootStormTime = Time.time;

        if (Time.time - lastRootStormTime >= rootStormCooldown)
        {
            lastRootStormTime = Time.time;
            return State.RootStorm;
        }
        
        return DecideRegularAttack();
    }

    private State DecideRegularAttack()
    {
        return Vector3.Distance(forestGolem.transform.position, ActorCache.Instance.Player.transform.position) <= stompRange
            ? RandomUtils.Choice(State.Stomp, State.Stomp, State.BoulderThrow)
            : State.BoulderThrow;
    }
}
