using System.Collections.Generic;
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
        descriptionText.text = GenerateDescription(RuneTooltipBuilder.Build(rune));
    }

    private string GenerateDescription(List<TooltipSegment> segments)
    {
        string description = "";

        foreach (TooltipSegment segment in segments)
        {
            switch (segment.Type)
            {
                case TooltipSegmentType.Text:
                    description += segment.Value;
                    break;

                case TooltipSegmentType.BoldText:
                    description += TextUtils.BoldText(segment.Value);
                    break;
            }
        }

        return description;
    }
}
