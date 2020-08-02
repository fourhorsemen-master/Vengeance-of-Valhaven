using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AbilityListingPanel : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private Image iconPanelImage = null;

    [SerializeField]
    private Text namePanelText = null;

    [SerializeField]
    private Text quantityPanelText = null;

    [SerializeField]
    private DraggableHighlighter highlighter = null;

    private AbilityReference ability;
    private int quantity;

    public void OnPointerEnter(PointerEventData _)
    {
        if (AbilityTreeEditorMenu.Instance.IsDraggingFromList) return;

        AbilityTooltip.Instance.Activate();
        AbilityTooltip.Instance.UpdateTooltip(ability);
        if (highlighter.HighlightState != DraggableHighlightState.Dragging)
            highlighter.HighlightState = DraggableHighlightState.Hover;
    }

    public void OnPointerExit(PointerEventData _)
    {
        AbilityTooltip.Instance.Deactivate();
        if (highlighter.HighlightState != DraggableHighlightState.Dragging)
            highlighter.HighlightState = DraggableHighlightState.Default;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        quantity -= 1;
        UpdateQuantityText();
        AbilityTreeEditorMenu.Instance.ListAbilityDragStartSubject.Next(ability);
        highlighter.HighlightState = DraggableHighlightState.Dragging;
    }

    /// <summary>
    /// We implement this method to satisfy the IDragHandler.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrag(PointerEventData eventData) { }

    public void OnEndDrag(PointerEventData eventData)
    {
        AbilityTreeEditorMenu.Instance.ListAbilityDragStopSubject.Next(ability);
        highlighter.HighlightState = DraggableHighlightState.Default;
    }

    public void Initialise(AbilityReference ability, int quantity)
    {
        this.ability = ability;
        this.quantity = quantity;

        highlighter.HighlightState = DraggableHighlightState.Default;

        iconPanelImage.sprite = AbilityIconManager.Instance.GetIcon(ability);

        namePanelText.text = AbilityLookup.Instance.GetAbilityDisplayName(ability);

        UpdateQuantityText();
    }

    private void UpdateQuantityText()
    {
        quantityPanelText.text = quantity.ToString();
    }
}
