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
    private UILineRenderer lineRenderer = null;

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

    public void ConnectTo(TreeAbility parentAbility)
    {
        ConnectTo(parentAbility.rectTransform);
    }

    public void ConnectToRoot(Image rootNodeOrb)
    {
        ConnectTo(rootNodeOrb.rectTransform);
    }

    private void ConnectTo(RectTransform parentRectTransform)
    {
        Vector2 parentRelativePosition = parentRectTransform.position - rectTransform.position;

        lineRenderer.Points = new Vector2[]
        {
            new Vector2(0f, 0f),
            parentRelativePosition / rectTransform.GetParentCanvas().scaleFactor
        };
    }
}
