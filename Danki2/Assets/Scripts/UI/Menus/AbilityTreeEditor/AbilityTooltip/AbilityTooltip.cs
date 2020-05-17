using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

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
    private TooltipAbilityOrb tooltipAbilityOrbPrefab = null;

    private PlayerTooltipBuilder tooltipBuilder;

    private void Start()
    {
        gameObject.SetActive(false);

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

    public void MoveToMouse()
    {
        var mousePos = Input.mousePosition;
        mousePos.x -= Screen.width / 2;
        mousePos.y -= Screen.height / 2;

        tooltipPanel.anchoredPosition = mousePos / tooltipPanel.GetParentCanvas().scaleFactor;
    }

    public void UpdateTooltip(Node node)
    {
        title.text = node.Ability.ToString();

        List<TooltipSegment> segments = tooltipBuilder.Build(node);
        description.text = GenerateDescription(segments);

        Vector2 newSizeDelta = tooltipPanel.sizeDelta;
        newSizeDelta.y = description.preferredHeight + 56;
        tooltipPanel.sizeDelta = newSizeDelta;

        Dictionary<OrbType, int> generatedOrbs =  AbilityLookup.Instance.GetGeneratedOrbs(node.Ability);
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

                case TooltipSegmentType.BaseNumericValue:
                    descriptionParts.Add(segment.Value);
                    break;

                case TooltipSegmentType.BonusNumericValue:
                    string bonus = $"+{segment.Value}";
                    string bonusWithColour = TextUtils.ColouredText("00ff00ff", bonus);

                    descriptionParts.Add($"({bonusWithColour})");
                    break;
            }
        }

        return string.Join(string.Empty, descriptionParts);
    }

    private void DisplayOrbs(Dictionary<OrbType, int> generatedOrbs)
    {
        for (int i = 0; i < abilityOrbPanel.childCount; i++)
        {
            Destroy(abilityOrbPanel.GetChild(i).gameObject);
        }

        foreach (OrbType key in Enum.GetValues(typeof(OrbType)))
        {
            if (!generatedOrbs.TryGetValue(key, out int count)) continue;

            for (int i = 0; i < generatedOrbs[key]; i++)
            {
                Instantiate(tooltipAbilityOrbPrefab, abilityOrbPanel.transform, false);
            }
        }
    }
}
