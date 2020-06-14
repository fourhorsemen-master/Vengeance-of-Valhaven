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

    public void OnPointerEnter(PointerEventData _)
    {
        if (AbilityTreeEditorMenu.Instance.DraggingAbility) return;

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
        AbilityTreeEditorMenu.Instance.AbilityDragStartSubject.Next();
    }

    public void OnDrag(PointerEventData eventData)
    {
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        AbilityTreeEditorMenu.Instance.AbilityDragStopSubject.Next();
    }

    public void Initialise(AbilityReference ability, int quantity)
    {
        this.ability = ability;

        SetHighlighted(false);

        iconPanelImage.sprite = AbilityIconManager.Instance.GetIcon(ability);

        namePanelText.text = AbilityLookup.Instance.GetAbilityDisplayName(ability);

        quantityPanelText.text = quantity.ToString();
    }

    private void SetHighlighted(bool highlighted)
    {
        abilityHighlight.enabled = highlighted;
    }
}
