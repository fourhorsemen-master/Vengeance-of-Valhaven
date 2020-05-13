using UnityEngine;

public class GameController : Singleton<GameController>
{
    private GameState gameState = GameState.Playing;
    private bool abilityTreeMenuButtonDown = false;

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
        abilityTreeMenuButtonDown = Input.GetAxis("AbilityTreeMenu") > 0;
    }
}
