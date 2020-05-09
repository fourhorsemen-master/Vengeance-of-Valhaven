using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityTooltip : Singleton<AbilityTooltip>
{
    [SerializeField]
    private RectTransform tooltipPanel = null;

    [SerializeField]
    private Text title = null;

    [SerializeField]
    private Text description = null;

    [SerializeField]
    private RectTransform abilityOrbPanel = null;

    [SerializeField]
    private TooltipAbilityOrb tooltipAbilityOrb = null;

    private PlayerTooltipBuilder tooltipBuilder;

    private void Start()
    {
        Player player = RoomManager.Instance.Player;
        tooltipBuilder = new PlayerTooltipBuilder(player);

        UpdateTooltip(player.AbilityTree.RootNode.GetChild(Direction.Left));
    }

    public void Activate()
    {
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    public void UpdateTooltip(Node node)
    {
        title.text = node.Ability.ToString();

        List<TooltipSegment> segments = tooltipBuilder.Build(node);
        description.text = GenerateDescription(segments);

        Vector2 newSizeDelta = tooltipPanel.sizeDelta;
        newSizeDelta.y = description.preferredHeight + 56;
        tooltipPanel.sizeDelta = newSizeDelta;

        Dictionary<OrbType, int> generatedOrbs =  AbilityLookup.GetGeneratedOrbs(node.Ability);
        DisplayOrbs(generatedOrbs);
    }

    private string GenerateDescription(List<TooltipSegment> segments)
    {
        List<string> descriptionParts = new List<string>();

        foreach (TooltipSegment segment in segments)
        {
            switch (segment.Type)
            {
                case TooltipSegmentType.Text:
                    descriptionParts.Add(segment.Value);
                    break;
            }
        }

        return string.Join(string.Empty, descriptionParts);
    }

    private void DisplayOrbs(Dictionary<OrbType, int> generatedOrbs)
    {
        return;
    }
}
