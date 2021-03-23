using UnityEngine;

public class GameplayStateController : Singleton<GameplayStateController>
{
    private GameplayState gameplayState = GameplayState.Playing;

    public BehaviourSubject<GameplayState> GameStateTransitionSubject { get; private set; }

    public GameplayState GameplayState
    {
        get => gameplayState;
        set
        {
            if (gameplayState == value) return;

            gameplayState = value;
            GameStateTransitionSubject.Next(gameplayState);
        }
    }

    protected override void Awake()
    {
        base.Awake();

        GameStateTransitionSubject = new BehaviourSubject<GameplayState>(gameplayState);
    }

    private void Update()
    {
        if (Input.GetButtonDown("AbilityTreeMenu"))
        {
            GameplayState = GameplayState == GameplayState.InAbilityTreeEditor
                ? GameplayState.Playing
                : GameplayState.InAbilityTreeEditor;
        }

        if (Input.GetButtonDown("AbilitySelectionMenu"))
        {
            GameplayState = GameplayState == GameplayState.InAbilitySelection
                ? GameplayState.Playing
                : GameplayState.InAbilitySelection;
        }
    }
}
