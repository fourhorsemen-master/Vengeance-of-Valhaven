using System.Collections.Generic;
using UnityEngine;

public class Tooltip<T> : Singleton<T> where T : MonoBehaviour
{
    [SerializeField]
    protected RectTransform tooltipPanel = null;

    protected ScreenQuadrant currentScreenQuadrant;

    private Dictionary<ScreenQuadrant, Vector2> pivotPoints = new Dictionary<ScreenQuadrant, Vector2>
    {
        { ScreenQuadrant.TopLeft, new Vector2(0, 0) },
        { ScreenQuadrant.TopRight, new Vector2(1, 0) },
        { ScreenQuadrant.BottomLeft, new Vector2(0, 1) },
        { ScreenQuadrant.BottomRight, new Vector2(1, 1) }
    };

    protected virtual void Update()
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

        currentScreenQuadrant = InputHelpers.GetMouseScreenQuadrant();
        tooltipPanel.pivot = pivotPoints[currentScreenQuadrant];

        MoveToMouse();
    }

    protected void DeactivateTooltip()
    {
        gameObject.SetActive(false);
    }

    private void MoveToMouse()
    {
        tooltipPanel.position = Input.mousePosition;
    }
}
