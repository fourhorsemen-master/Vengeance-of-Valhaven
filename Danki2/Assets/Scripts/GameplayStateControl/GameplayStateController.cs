public class GameplayStateController : Singleton<GameplayStateController>
{
    public BehaviourSubject<GameplayState> GameStateTransitionSubject { get; } = new BehaviourSubject<GameplayState>(GameplayState.Playing);

    public GameplayState GameplayState
    {
        get => GameStateTransitionSubject.Value;
        set
        {
            if (GameplayState == value) return;
            GameStateTransitionSubject.Next(value);
        }
    }
}
