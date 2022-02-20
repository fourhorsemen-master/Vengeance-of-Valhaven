using UnityEngine;

public class ForestGolemSpawnEnts : IStateMachineComponent
{
    private readonly ForestGolem forestGolem;
    private readonly int entCount;
    private readonly PositionFinder positionFinder;

    public ForestGolemSpawnEnts(ForestGolem forestGolem, Actor target, int entCount)
    {
        this.forestGolem = forestGolem;
        this.entCount = entCount;
        positionFinder = new PositionFinder(forestGolem, target, 5f, 5f);
    }

    public void Enter()
    {
        Repeat.Times(entCount, () =>
        {
            Vector3 position = positionFinder.GetRandomPositionAroundTarget();
            forestGolem.SpawnEnt(ActorCache.Instance.Player.transform.position);
        });
    }

    public void Exit() {}
    public void Update() {}
}
