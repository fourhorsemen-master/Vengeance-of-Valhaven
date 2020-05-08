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

        //tooltipPanel.sizeDelta = description.preferredHeight

        Dictionary<OrbType, int> generatedOrbs =  AbilityLookup.GetGeneratedOrbs(node.Ability);
        DisplayOrbs(generatedOrbs);
    }

    private string GenerateDescription(List<TooltipSegment> segments)
    {
        throw new NotImplementedException();
    }

    private void DisplayOrbs(Dictionary<OrbType, int> generatedOrbs)
    {
        throw new NotImplementedException();
    }
}
