using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AbilityListingPanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private Image iconPanelImage = null;

    [SerializeField]
    private Text namePanelText = null;

    [SerializeField]
    private Text quantityPanelText = null;

    private AbilityReference ability;

    public void OnPointerEnter(PointerEventData _)
    {
        AbilityTooltip.Instance.Activate();
        AbilityTooltip.Instance.UpdateTooltip(ability);
    }

    public void OnPointerExit(PointerEventData _) => AbilityTooltip.Instance.Deactivate();

    public void Initialise(AbilityReference ability, int quantity)
    {
        this.ability = ability;

        iconPanelImage.sprite = AbilityIconManager.Instance.GetIcon(ability);

        namePanelText.text = AbilityLookup.Instance.GetAbilityDisplayName(ability);

        quantityPanelText.text = quantity.ToString();
    }
}
