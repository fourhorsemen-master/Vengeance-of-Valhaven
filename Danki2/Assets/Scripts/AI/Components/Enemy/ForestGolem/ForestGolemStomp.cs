public class ForestGolemStomp : IStateMachineComponent
{
    private readonly ForestGolem forestGolem;

    public ForestGolemStomp(ForestGolem forestGolem)
    {
        this.forestGolem = forestGolem;
    }

    public void Enter() => forestGolem.Stomp();
    public void Exit() {}
    public void Update() {}
}
