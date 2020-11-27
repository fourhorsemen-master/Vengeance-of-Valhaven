using System.Collections.Generic;
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

    private static readonly Dictionary<int, string> romanNumeralLookup = new Dictionary<int, string>
    {
        {0, "0"},
        {1, "I"},
        {2, "II"},
        {3, "III"},
        {4, "IV"},
        {5, "V"},
        {6, "VI"},
        {7, "VII"},
        {8, "VIII"},
        {9, "IX"},
        {10, "X"}
    };

    public void Initialise(string title, string tooltip, int requiredTreeDepth, bool bonusEnabled)
    {
        Color textColor = bonusEnabled ? enabledTextColour : disabledTextColour;

        titleText.text = $"{romanNumeralLookup[requiredTreeDepth]} - {title}";
        titleText.color = textColor;

        descriptionText.text = tooltip;
        descriptionText.color = textColor;
    }
}
