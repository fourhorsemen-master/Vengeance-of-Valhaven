using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class TreeAbility : MonoBehaviour
{
    [SerializeField]
    private RectTransform rectTransform = null;

    [SerializeField]
    private Image abilityImage = null;

    [SerializeField]
    private Image abilityOverlay = null;

    [SerializeField]
    private UILineRenderer leftChildLineRenderer = null;

    [SerializeField]
    private UILineRenderer rightChildLineRenderer = null;

    public void ShiftRight(float amount)
    {
        Vector3 newPosition = rectTransform.localPosition;
        newPosition += Vector3.right * amount;
        rectTransform.localPosition = newPosition;
    }

    public void SetImage(Sprite sprite)
    {
        abilityImage.sprite = sprite;
    }

    public void ConnectToChild(TreeAbility child, Direction direction)
    {
        Vector2 childRelativePosition = (child.rectTransform.position - rectTransform.position) / rectTransform.GetParentCanvas().scaleFactor;

        Vector2[] points = new Vector2[]
        {
            new Vector2(0f, 0f),
            childRelativePosition
        };

        switch (direction)
        {
            case Direction.Left:
                leftChildLineRenderer.Points = points;
                break;

            case Direction.Right:
                rightChildLineRenderer.Points = points;
                break;
        }
    }

    internal void RemoveOverlay()
    {
        abilityOverlay.enabled = false;
    }
}
