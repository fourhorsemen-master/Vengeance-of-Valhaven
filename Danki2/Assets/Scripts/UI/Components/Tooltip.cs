using UnityEngine;
using UnityEngine.UI.Extensions;

public class Tooltip<T> : Singleton<T> where T : MonoBehaviour
{
    [SerializeField]
    protected RectTransform tooltipPanel = null;

    private void Update()
    {
        if (gameObject.activeInHierarchy)
        {
            MoveToMouse();
        }
    }

    private void OnDisable()
    {
        // This is to avoid the tooltip being displayed if the menu is closed and reopened with the mouse no longer over an ability.
        DeactivateTooltip();
    }

    protected void ActivateTooltip()
    {
        gameObject.SetActive(true);
        MoveToMouse();
    }

    protected void DeactivateTooltip()
    {
        gameObject.SetActive(false);
    }

    private void MoveToMouse()
    {
        Vector3 newPosition = Input.mousePosition;

        Vector2 tooltipPanelSize = tooltipPanel.sizeDelta * tooltipPanel.GetParentCanvas().scaleFactor;

        float xOverlap = Mathf.Max(0f, newPosition.x + tooltipPanelSize.x - Screen.width);
        float yOverlap = Mathf.Max(0f, newPosition.y + tooltipPanelSize.y - Screen.height);

        newPosition.x -= xOverlap;
        newPosition.y -= yOverlap;

        tooltipPanel.position = newPosition;
    }
}
