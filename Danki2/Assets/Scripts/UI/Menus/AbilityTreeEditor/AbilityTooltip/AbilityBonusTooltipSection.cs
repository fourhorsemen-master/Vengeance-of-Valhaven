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

    [SerializeField]
    private OrbGenerationPanel requiredOrbsPanel = null;

    public void Initialise(string title, string tooltip, OrbCollection requiredOrbs, OrbCollection providedOrbs = null)
    {
        bool bonusEnabled = providedOrbs == null || providedOrbs.IsSuperset(requiredOrbs);
        Color textColor = bonusEnabled ? enabledTextColour : disabledTextColour;

        titleText.text = $"Bonus: {title}";
        titleText.color = textColor;

        descriptionText.text = tooltip;
        descriptionText.color = textColor;

        requiredOrbsPanel.DisplayOrbs(requiredOrbs, providedOrbs);
    }
}
