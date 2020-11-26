using UnityEngine;

public class StaticUI : Singleton<StaticUI>
{
    [SerializeField]
    private CanvasGroup canvasGroup = null;

    private void Start()
    {
        GameStateController.Instance.GameStateTransitionSubject.Subscribe(gameState =>
        {
            bool visible = gameState == GameState.Playing;
            canvasGroup.alpha = visible ? 1 : 0;
        });
    }
}
