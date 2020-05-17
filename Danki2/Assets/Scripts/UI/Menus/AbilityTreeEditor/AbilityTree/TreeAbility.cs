using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class TreeAbility : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private RectTransform rectTransform = null;

    [SerializeField]
    private Sprite rootNodeSprite = null;

    [SerializeField]
    private Image abilityImage = null;

    [SerializeField]
    private Image abilityOverlay = null;

    [SerializeField]
    private UILineRenderer leftChildLineRenderer = null;

    [SerializeField]
    private UILineRenderer rightChildLineRenderer = null;

    public Subject MouseEnterSubject = new Subject();
    public Subject MouseLeaveSubject = new Subject();

    public void OnPointerEnter(PointerEventData eventData) => MouseEnterSubject.Next();

    public void OnPointerExit(PointerEventData eventData) => MouseLeaveSubject.Next();

    public void ShiftRight(float amount)
    {
        Vector3 newPosition = rectTransform.localPosition;
        newPosition += Vector3.right * amount;
        rectTransform.localPosition = newPosition;
    }

    public void SetNode(Node node)
    {
        if (node.IsRootNode())
        {
            abilityImage.sprite = rootNodeSprite;
            RemoveOverlay();
        }
        else
        {
            abilityImage.sprite = AbilityIconManager.Instance.GetIcon(node.Ability);
        }
    }

    public void ConnectToChild(TreeAbility child, Direction direction)
    {
        Vector2 childRelativePosition = (child.rectTransform.position - rectTransform.position) / rectTransform.GetParentCanvas().scaleFactor;

        Vector2[] points = { new Vector2(0f, 0f), childRelativePosition };

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

    public void RemoveOverlay()
    {
        abilityOverlay.enabled = false;
    }
}
