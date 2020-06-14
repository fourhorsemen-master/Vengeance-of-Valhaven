using System;
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
    private Image abilityHighlight = null;

    [SerializeField]
    private UILineRenderer leftChildLineRenderer = null;

    [SerializeField]
    private UILineRenderer rightChildLineRenderer = null;

    [SerializeField]
    private AbilityInsertListener abilityInsertListener = null;

    private Node node;

    private void Start()
    {
        abilityInsertListener.SetInsertableAreas(node);

        abilityInsertListener.AbilityInsertSubject.Subscribe(location =>
        {
            Debug.Log(location.ToString());

            if (AbilityTreeEditorMenu.Instance.DraggingAbility)
                {
                    // Set the ability.
                }
            }
        );
    }

    public void OnPointerEnter(PointerEventData _) {
        if (node.IsRootNode) return;

        AbilityTooltip.Instance.Activate();
        AbilityTooltip.Instance.UpdateTooltip(node);
        SetHighlighted(true);
    }

    public void OnPointerExit(PointerEventData _)
    {
        AbilityTooltip.Instance.Deactivate();
        SetHighlighted(false);
    }

    public void ShiftRight(float amount)
    {
        Vector3 newPosition = rectTransform.localPosition;
        newPosition += Vector3.right * amount;
        rectTransform.localPosition = newPosition;
    }

    public void SetNode(Node node)
    {
        this.node = node;
        SetHighlighted(false);

        if (node.IsRootNode)
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

    private void RemoveOverlay()
    {
        abilityOverlay.enabled = false;
    }

    private void SetHighlighted(bool highlighted)
    {
        abilityHighlight.enabled = highlighted;
    }
}
