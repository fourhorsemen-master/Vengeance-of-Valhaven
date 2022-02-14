using System.Collections.Generic;
using UnityEngine;

public class Tooltip : MonoBehaviour
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
        MoveToMouse();
    }

    public static TTooltip Create<TTooltip>(TTooltip prefab) where TTooltip : Tooltip
    {
        // Parenting to MenuController ensures we are at the end of the heirarchy and appearing at the top
        return Instantiate(prefab, MenuController.Instance.transform);
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    protected void ActivateTooltip()
    {
        currentScreenQuadrant = InputHelpers.GetMouseScreenQuadrant();
        tooltipPanel.pivot = pivotPoints[currentScreenQuadrant];

        MoveToMouse();
    }

    private void MoveToMouse()
    {
        tooltipPanel.position = Input.mousePosition;
    }
}
