using UnityEngine;
using UnityEngine.UI;

public class AbilityPanel : MonoBehaviour
{
    [SerializeField]
    private Image iconPanelImage;

    [SerializeField]
    private Text namePanelText;

    [SerializeField]
    private Text quantityPanelText;

    public void InitialisePanel(AbilityReference ability, int quantity)
    {
        iconPanelImage.sprite = AbilityIconManager.Instance.GetIcon(ability);

        namePanelText.text = System.Enum.GetName(typeof(AbilityReference), ability);

        quantityPanelText.text = quantity.ToString();
    }
}
