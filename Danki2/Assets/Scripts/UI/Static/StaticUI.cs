using UnityEngine;

public class StaticUI : Singleton<StaticUI>
{
    [SerializeField]
    private CanvasGroup canvasGroup = null;

    private void Start()
    {
        GameplayStateController.Instance.GameStateTransitionSubject.Subscribe(gameState =>
        {
            bool visible = gameState == GameplayState.Playing;
            canvasGroup.alpha = visible ? 1 : 0;
        });
    }
}
