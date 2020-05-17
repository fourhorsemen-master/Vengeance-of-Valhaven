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
    private Transform orbGenerattionPanel = null;

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

    private void Update()
    {
        MoveToMouse();
    }

    public void Activate()
    {
        gameObject.SetActive(true);
        MoveToMouse();
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

        description.rectTransform.sizeDelta = new Vector2(
            description.rectTransform.sizeDelta.x,
            description.preferredHeight
        );

        Dictionary<OrbType, int> generatedOrbs = AbilityLookup.Instance.GetGeneratedOrbs(node.Ability);
        bool generatesOrbs = DisplayOrbs(generatedOrbs);

        float newHeight = generatesOrbs
            ? description.preferredHeight + 48
            : description.preferredHeight + 24;

        Vector2 newSizeDelta = tooltipPanel.sizeDelta;
        newSizeDelta.y = description.preferredHeight + newHeight;
        tooltipPanel.sizeDelta = newSizeDelta;
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

    private bool DisplayOrbs(Dictionary<OrbType, int> generatedOrbs)
    {
        for (int i = 0; i < abilityOrbPanel.childCount; i++)
        {
            Destroy(abilityOrbPanel.GetChild(i).gameObject);
        }

        bool generatesOrbs = false;

        foreach (OrbType key in Enum.GetValues(typeof(OrbType)))
        {
            if (!generatedOrbs.TryGetValue(key, out int count)) continue;

            for (int i = 0; i < generatedOrbs[key]; i++)
            {
                Instantiate(tooltipAbilityOrbPrefab, abilityOrbPanel.transform, false);
                generatesOrbs = true;
            }
        }

        return generatesOrbs;
    }

    private void MoveToMouse()
    {
        tooltipPanel.position = Input.mousePosition;
    }
}
