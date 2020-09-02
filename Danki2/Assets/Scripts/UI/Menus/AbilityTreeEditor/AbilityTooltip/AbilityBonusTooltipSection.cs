using UnityEngine;
using UnityEngine.UI;

public class AbilityBonusTooltipSection : MonoBehaviour
{
    [SerializeField]
    private Color enabledTextColour = Color.white;

    [SerializeField]
    private Color disabledTextColour = Color.grey;

    [SerializeField]
    private RectTransform rectTransform = null;

    [SerializeField]
    private Text titleText = null;

    [SerializeField]
    private Text descriptionText = null;

    [SerializeField]
    private OrbGenerationPanel requiredOrbsPanel = null;



    public void Initialise(string title, string tooltip, OrbCollection requiredOrbs, OrbCollection providedOrbs = null)
    {
        bool bonusEnabled = providedOrbs == null || providedOrbs.IsSuperset(requiredOrbs);
        var textColor = bonusEnabled ? enabledTextColour : disabledTextColour;

        titleText.text = $"Bonus: {title}";
        titleText.color = textColor;

        descriptionText.text = tooltip;
        descriptionText.color = textColor;

        requiredOrbsPanel.DisplayOrbs(requiredOrbs, providedOrbs);
    }

    public float GetSectionHeight()
    {
        descriptionText.rectTransform.sizeDelta = new Vector2(
            descriptionText.rectTransform.sizeDelta.x,
            descriptionText.preferredHeight
        );

        float newHeight = descriptionText.preferredHeight + 30f;

        rectTransform.sizeDelta = new Vector2(
            rectTransform.sizeDelta.x,
            newHeight
        );

        return newHeight + 8f;
    }
}
