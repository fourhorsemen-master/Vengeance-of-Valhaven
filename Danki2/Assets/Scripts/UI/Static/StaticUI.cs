using UnityEngine;

public abstract class StaticUI<T> : Singleton<T> where T : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup canvasGroup = null;

    private float visibility = 1;
    private float? visibilityOverride = null;

    private void Start()
    {
        GameplayStateController.Instance.GameStateTransitionSubject.Subscribe(gameState =>
        {
            visibility = gameState == GameplayState.Playing ? 1 : 0;
            UpdateVisibility();
        });
    }

    public void OverrideVisibility(float visibilityOverride)
    {
        this.visibilityOverride = visibilityOverride;
        UpdateVisibility();
    }

    public void RemoveVisibilityOverride()
    {
        visibilityOverride = null;
        UpdateVisibility();
    }

    private void UpdateVisibility()
    {
        canvasGroup.alpha = visibilityOverride ?? visibility;
    }
}
