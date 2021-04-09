public class GameplayStateComponent : IStateMachineComponent
{
    private readonly GameplayState gameplayState;

    public GameplayStateComponent(GameplayState gameplayState)
    {
        this.gameplayState = gameplayState;
    }

    public void Enter() => GameplayStateController.Instance.GameplayState = gameplayState;
    public void Exit() {}
    public void Update() {}
}
