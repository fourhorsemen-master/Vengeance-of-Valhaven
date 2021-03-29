using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class TreeAbility : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
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
    private DraggableHighlighter highlighter = null;

    [SerializeField]
    private UILineRenderer leftChildLineRenderer = null;

    [SerializeField]
    private UILineRenderer rightChildLineRenderer = null;

    [SerializeField]
    private AbilityInsertListener abilityInsertListener = null;

    private Node node;
    private Subscription<AbilityReference> dragStartSubscription;
    private Subscription<AbilityReference> dragStopSubscription;

    private AbilityTooltip tooltip;
    
    private void Start()
    {
        dragStartSubscription = AbilityTreeEditorMenu.Instance.ListAbilityDragStartSubject
            .Subscribe(ability => abilityInsertListener.Activate(node, ability));

        dragStopSubscription = AbilityTreeEditorMenu.Instance.ListAbilityDragStopSubject
            .Subscribe(ability => abilityInsertListener.Deactivate());

        abilityInsertListener.AbilityInsertSubject.Subscribe(location =>
        {
            node.Insert(AbilityTreeEditorMenu.Instance.AbilityDraggingFromList, location);
        });
    }

    private void OnDisable()
    {
        dragStartSubscription?.Unsubscribe();
        dragStopSubscription?.Unsubscribe();
        if (tooltip) tooltip.Destroy();
    }

    public void OnPointerEnter(PointerEventData _)
    {
        if (node.IsRootNode || AbilityTreeEditorMenu.Instance.IsDraggingFromList) return;

        AbilityTreeEditorMenu.Instance.CurrentTreeNodeHover = node;

        tooltip = AbilityTreeEditorMenu.Instance.CreateTooltip(node);
        if (highlighter.HighlightState != DraggableHighlightState.Dragging)
            highlighter.HighlightState = DraggableHighlightState.Hover;
    }

    public void OnPointerExit(PointerEventData _)
    {
        AbilityTreeEditorMenu.Instance.CurrentTreeNodeHover = null;
        if (tooltip) tooltip.Destroy();
        if (highlighter.HighlightState != DraggableHighlightState.Dragging)
            highlighter.HighlightState = DraggableHighlightState.Default;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        highlighter.HighlightState = DraggableHighlightState.Dragging;
    }

    /// <summary>
    /// We implement this method to satisfy the IDragHandler.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrag(PointerEventData eventData) { }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (node.IsRootNode)
        {
            return;
        }
        else if (AbilityTreeEditorMenu.Instance.CurrentTreeNodeHover == null)
        {
            node.RemoveSelfAndDescendants();
        }
        else
        {
            Node otherNode = AbilityTreeEditorMenu.Instance.CurrentTreeNodeHover;

            if (node.CanSwapAbilitiesWith(otherNode))
                node.SwapAbilitiesWith(otherNode);
        }

        AbilityTreeEditorMenu.Instance.TreeAbilityDragStopSubject.Next();
        highlighter.HighlightState = DraggableHighlightState.Default;
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
        highlighter.HighlightState = DraggableHighlightState.Default;

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
}
