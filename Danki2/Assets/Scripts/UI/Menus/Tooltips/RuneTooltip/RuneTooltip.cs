using UnityEngine;
using UnityEngine.UI;

public class RuneTooltip : Tooltip
{
    [SerializeField] private Text titleText = null;
    [SerializeField] private Text descriptionText = null;

    public static RuneTooltip Create(Rune rune, Transform transform)
    {
        RuneTooltip runeTooltip = Instantiate(TooltipLookup.Instance.RuneTooltip, transform);
        runeTooltip.Activate(rune);
        return runeTooltip;
    }

    private void Activate(Rune rune)
    {
        ActivateTooltip();
        
        titleText.text = RuneLookup.Instance.GetDisplayName(rune);
        descriptionText.text = RuneLookup.Instance.GetTooltip(rune);
    }
}
