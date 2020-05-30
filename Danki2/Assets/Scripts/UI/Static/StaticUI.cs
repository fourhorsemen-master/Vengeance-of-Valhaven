public class StaticUI : Singleton<StaticUI>
{
    private void Start()
    {
        GameStateController.Instance.GameStateTransitionSubject.Subscribe(gameState =>
            gameObject.SetActive(gameState == GameState.Playing)
        );
    }
}
