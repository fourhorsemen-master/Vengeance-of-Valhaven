using UnityEngine;
using UnityEngine.UI;

public class RuneTooltip : Tooltip
{
    [SerializeField] private Text titleText = null;
    [SerializeField] private Text descriptionText = null;

    [SerializeField] private SupplementaryTooltipPanel abilitySupplementaryTooltipPanel = null;

    public static RuneTooltip Create(Rune rune)
    {
        RuneTooltip runeTooltip = Create(TooltipLookup.Instance.RuneTooltip);
        runeTooltip.Activate(rune);

        return runeTooltip;
    }

    private void Activate(Rune rune)
    {
        ActivateTooltip();
        
        titleText.text = RuneLookup.Instance.GetDisplayName(rune);
        descriptionText.text = RuneLookup.Instance.GetTooltip(rune);

        ActivateSupplementaryTooltips(rune);
    }

    private void ActivateSupplementaryTooltips(Rune rune)
    {
        abilitySupplementaryTooltipPanel.Activate(
            activeEffects: RuneLookup.Instance.GetActiveEffects(rune),
            passiveEffects: RuneLookup.Instance.GetPassiveEffects(rune),
            stackingEffects: RuneLookup.Instance.GetStackingEffects(rune)
        );
    }
}
