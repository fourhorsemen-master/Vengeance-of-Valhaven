public class ForestGolemThrowBoulder : IStateMachineComponent
{
    private readonly ForestGolem forestGolem;

    public ForestGolemThrowBoulder(ForestGolem forestGolem)
    {
        this.forestGolem = forestGolem;
    }

    public void Enter() => forestGolem.ThrowBoulder(ActorCache.Instance.Player.transform.position);
    public void Exit() {}
    public void Update() {}
}
