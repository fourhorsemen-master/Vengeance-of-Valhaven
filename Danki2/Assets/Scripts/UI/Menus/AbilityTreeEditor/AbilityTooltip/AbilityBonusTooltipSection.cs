using UnityEngine;
using UnityEngine.UI;

public class AbilityBonusTooltipSection : MonoBehaviour
{
    [SerializeField]
    private Color enabledTextColour = Color.white;

    [SerializeField]
    private Color disabledTextColour = Color.grey;

    [SerializeField]
    private Text titleText = null;

    [SerializeField]
    private Text descriptionText = null;

    public void Initialise(string title, string tooltip)
    {
        bool bonusEnabled = true;
        Color textColor = bonusEnabled ? enabledTextColour : disabledTextColour;

        titleText.text = $"Bonus: {title}";
        titleText.color = textColor;

        descriptionText.text = tooltip;
        descriptionText.color = textColor;
    }
}
