using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityTooltip : Singleton<AbilityTooltip>
{
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

    public void UpdateTooltip(Node node)
    {
        title.text = node.Ability.ToString();

        List<TooltipSegment> segments = tooltipBuilder.Build(node);

    }

    public void Activate()
    {
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
