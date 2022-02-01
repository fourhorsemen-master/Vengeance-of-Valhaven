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

    private Ability ability;

    private AbilityTooltip tooltip;

    private void OnDisable()
    {
        if (tooltip) tooltip.Destroy();
    }

    public void OnPointerEnter(PointerEventData _)
    {
        if (AbilityTreeEditorMenu.Instance.IsDraggingFromList) return;

        tooltip = AbilityTreeEditorMenu.Instance.CreateTooltip(ability);
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

    public void Initialise(Ability ability)
    {
        this.ability = ability;

        highlighter.HighlightState = DraggableHighlightState.Default;

        iconPanelImage.sprite = AbilityTypeLookup.Instance.GetAbilityIcon(ability.Type);

        namePanelText.text = ability.DisplayName;
        namePanelText.color = RarityLookup.Instance.Lookup[ability.Rarity].Colour;

        empowermentPanel.SetEmpowerments(ability);

        UpdateQuantityText();
    }

    private void UpdateQuantityText()
    {
        quantityPanelText.text = "1";
    }
}
