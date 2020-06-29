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
    private Image abilityHighlight = null;

    private AbilityReference ability;
    private int quantity;

    public Subject DragEndSubject { get; } = new Subject();

    public void OnPointerEnter(PointerEventData _)
    {
        if (AbilityTreeEditorMenu.Instance.IsDragging) return;

        AbilityTooltip.Instance.Activate();
        AbilityTooltip.Instance.UpdateTooltip(ability);
        SetHighlighted(true);
    }

    public void OnPointerExit(PointerEventData _)
    {
        AbilityTooltip.Instance.Deactivate();
        SetHighlighted(false);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        quantity -= 1;
        UpdateQuantityText();
        AbilityTreeEditorMenu.Instance.AbilityDragStartSubject.Next(ability);
    }

    /// <summary>
    /// We implement this method to satisfy the IDragHandler.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrag(PointerEventData eventData) { }

    public void OnEndDrag(PointerEventData eventData)
    {
        DragEndSubject.Next();
        AbilityTreeEditorMenu.Instance.AbilityDragStopSubject.Next(ability);
    }

    public void Initialise(AbilityReference ability, int quantity)
    {
        this.ability = ability;
        this.quantity = quantity;

        SetHighlighted(false);

        iconPanelImage.sprite = AbilityIconManager.Instance.GetIcon(ability);

        namePanelText.text = AbilityLookup.Instance.GetAbilityDisplayName(ability);

        UpdateQuantityText();
    }

    private void UpdateQuantityText()
    {
        quantityPanelText.text = quantity.ToString();
    }

    private void SetHighlighted(bool highlighted)
    {
        abilityHighlight.enabled = highlighted;
    }
}
