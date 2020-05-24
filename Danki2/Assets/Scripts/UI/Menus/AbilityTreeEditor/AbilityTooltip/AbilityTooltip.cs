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
    private TooltipAbilityOrb tooltipAbilityOrbPrefab = null;

    private PlayerTooltipBuilder tooltipBuilder;

    // TODO: include this in an OrbType lookup
    private Dictionary<OrbType?, string> orbColourMap;

    private void Start()
    {
        gameObject.SetActive(false);

        Player player = RoomManager.Instance.Player;
        tooltipBuilder = new PlayerTooltipBuilder(player);

        orbColourMap = new Dictionary<OrbType?, string> {
            { OrbType.Aggression, "ff5555ff" },
            { OrbType.Balance, "55ff55ff" },
            { OrbType.Cunning, "5555ffff" },
        };
    }

    private void Update()
    {
        MoveToMouse();
    }

    private void OnDisable()
    {
        // This is to avoid the tooltip being displayed if the menu is closed and reopened with the mouse no longer over an ability.
        Deactivate();
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

        AbilityLookup.Instance.TryGetAbilityOrbType(node.Ability, out OrbType abilityOrbType);

        List<TooltipSegment> segments = tooltipBuilder.Build(node);
        description.text = GenerateDescription(abilityOrbType, segments);

        description.rectTransform.sizeDelta = new Vector2(
            description.rectTransform.sizeDelta.x,
            description.preferredHeight
        );

        OrbCollection generatedOrbs = AbilityLookup.Instance.GetGeneratedOrbs(node.Ability);
        DisplayOrbs(generatedOrbs);

        float newHeight = description.preferredHeight + (generatedOrbs.IsEmpty ? 36f : 60f);

        tooltipPanel.sizeDelta = new Vector2(
            tooltipPanel.sizeDelta.x,
            newHeight
        );
    }

    private string GenerateDescription(OrbType abilityType, List<TooltipSegment> segments)
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
                    string colorHex = orbColourMap[abilityType];
                    string bonusWithColour = TextUtils.ColouredText(colorHex, bonus);

                    descriptionParts.Add($" ({bonusWithColour})");
                    break;
            }
        }

        return string.Join(string.Empty, descriptionParts);
    }

    private void DisplayOrbs(OrbCollection generatedOrbs)
    {
        for (int i = 0; i < abilityOrbPanel.childCount; i++)
        {
            Destroy(abilityOrbPanel.GetChild(i).gameObject);
        }

        generatedOrbs.ForEachOrb(orbType => {
            TooltipAbilityOrb orb = Instantiate(tooltipAbilityOrbPrefab, abilityOrbPanel.transform, false);
            orb.SetType(orbType);
        });
    }

    private void MoveToMouse()
    {
        tooltipPanel.position = Input.mousePosition;
    }
}
