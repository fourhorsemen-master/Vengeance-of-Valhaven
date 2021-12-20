using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AbilityListingPanel : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image iconPanelImage = null;

    [SerializeField] private Text namePanelText = null;

    [SerializeField] private Text quantityPanelText = null;

    [SerializeField] private DraggableHighlighter highlighter = null;

    [SerializeField] private AbilityEmpowermentContainer empowermentPanel = null;

    private SerializableGuid abilityId;
    private int quantity;

    private AbilityTooltip tooltip;

    private void OnDisable()
    {
        if (tooltip) tooltip.Destroy();
    }

    public void OnPointerEnter(PointerEventData _)
    {
        if (AbilityTreeEditorMenu.Instance.IsDraggingFromList) return;

        tooltip = AbilityTreeEditorMenu.Instance.CreateTooltip(abilityId);
        if (highlighter.HighlightState != DraggableHighlightState.Dragging)
            highlighter.HighlightState = DraggableHighlightState.Hover;
    }

    public void OnPointerExit(PointerEventData _)
    {
        if (tooltip) tooltip.Destroy();
        if (highlighter.HighlightState != DraggableHighlightState.Dragging)
            highlighter.HighlightState = DraggableHighlightState.Default;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        quantity -= 1;
        UpdateQuantityText();
        AbilityTreeEditorMenu.Instance.ListAbilityDragStartSubject.Next(abilityId);
        highlighter.HighlightState = DraggableHighlightState.Dragging;
    }

    /// <summary>
    /// We implement this method to satisfy the IDragHandler.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrag(PointerEventData eventData) { }

    public void OnEndDrag(PointerEventData eventData)
    {
        AbilityTreeEditorMenu.Instance.ListAbilityDragStopSubject.Next(abilityId);
        highlighter.HighlightState = DraggableHighlightState.Default;
    }

    public void Initialise(SerializableGuid abilityId, int quantity)
    {
        this.abilityId = abilityId;
        this.quantity = quantity;

        highlighter.HighlightState = DraggableHighlightState.Default;

        iconPanelImage.sprite = AbilityLookup.Instance.GetIcon(abilityId);

        namePanelText.text = AbilityLookup.Instance.GetDisplayName(abilityId);
        namePanelText.color = RarityLookup.Instance.Lookup[AbilityLookup.Instance.GetRarity(abilityId)].Colour;

        empowermentPanel.SetEmpowerments(abilityId);

        UpdateQuantityText();
    }

    private void UpdateQuantityText()
    {
        quantityPanelText.text = quantity.ToString();
    }
}
