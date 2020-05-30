using UnityEngine;
using UnityEngine.UI;

public class AbilityListingPanel : MonoBehaviour
{
    [SerializeField]
    private Image iconPanelImage = null;

    [SerializeField]
    private Text namePanelText = null;

    [SerializeField]
    private Text quantityPanelText = null;

    public void Initialise(AbilityReference ability, int quantity)
    {
        iconPanelImage.sprite = AbilityIconManager.Instance.GetIcon(ability);

        namePanelText.text = AbilityLookup.Instance.GetAbilityDisplayName(ability);

        quantityPanelText.text = quantity.ToString();
    }
}
