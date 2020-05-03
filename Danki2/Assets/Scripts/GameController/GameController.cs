using UnityEngine;

public class GameController : Singleton<GameController>
{
    [SerializeField]
    private GameObject StaticUi = null;

    public GameState GameState { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        GameState = GameState.Playing;
    }

    public void SetGameState(GameState newGameState)
    {
        GameState = newGameState;

        StaticUi.SetActive(newGameState == GameState.Playing);
    }
}
