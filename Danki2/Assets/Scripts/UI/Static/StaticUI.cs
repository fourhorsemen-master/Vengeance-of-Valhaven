public class StaticUI : Singleton<StaticUI>
{
    private void Start()
    {
        GameController.Instance.GameStateTransitionSubject.Subscribe(gameState =>
            gameObject.SetActive(gameState == GameState.Playing)
        );
    }
}
