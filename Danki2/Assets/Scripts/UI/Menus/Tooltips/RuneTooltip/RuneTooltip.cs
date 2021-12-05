using UnityEngine;
using UnityEngine.UI;

public class RuneTooltip : Tooltip
{
    [SerializeField] private Text titleText = null;
    [SerializeField] private Text descriptionText = null;

    [SerializeField] private SupplementaryTooltipPanel abilitySupplementaryTooltipPanel = null;

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
