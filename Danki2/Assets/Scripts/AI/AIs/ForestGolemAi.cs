using UnityEngine;

public class ForestGolemAi : Ai
{
    [SerializeField] private ForestGolem forestGolem = null;

    protected override Actor Actor => forestGolem;
    
    protected override IStateMachineComponent BuildStateMachineComponent()
    {
        return new MoveTowardsAtDistance(forestGolem, ActorCache.Instance.Player, 3);
    }
}
