using UnityEngine;

public class GameStateController : Singleton<GameStateController>
{
    private GameState gameState = GameState.Playing;

    public BehaviourSubject<GameState> GameStateTransitionSubject { get; private set; }

    public GameState GameState
    {
        get => gameState;
        set
        {
            if (gameState == value) return;

            gameState = value;
            GameStateTransitionSubject.Next(gameState);
        }
    }

    protected override void Awake()
    {
        base.Awake();

        GameStateTransitionSubject = new BehaviourSubject<GameState>(gameState);
    }

    private void Update()
    {
        if (Input.GetButtonDown("AbilityTreeMenu"))
        {
            GameState = GameState == GameState.InAbilityTreeEditor
                ? GameState.Playing
                : GameState.InAbilityTreeEditor;
        }
    }
}
