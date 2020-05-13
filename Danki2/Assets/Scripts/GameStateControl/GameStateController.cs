using UnityEngine;

public class GameStateController : Singleton<GameStateController>
{
    private GameState gameState = GameState.Playing;
    private bool abilityTreeMenuButtonDownLastFrame;

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
        bool abilityTreeMenuButtonDown = Input.GetAxisRaw("AbilityTreeMenu") == 1;

        if (abilityTreeMenuButtonDown && !abilityTreeMenuButtonDownLastFrame)
        {
            GameState = GameState == GameState.InAbilityTreeEditor
                ? GameState.Playing
                : GameState.InAbilityTreeEditor;
        }

        abilityTreeMenuButtonDownLastFrame = abilityTreeMenuButtonDown;
    }
}
