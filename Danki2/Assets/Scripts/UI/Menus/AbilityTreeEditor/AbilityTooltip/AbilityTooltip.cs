using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class AbilityTooltip : Tooltip<AbilityTooltip>
{
    [SerializeField]
    private Text titleText = null;

    [SerializeField]
    private Text finisherText = null;

    [SerializeField]
    private Text descriptionText = null;

    [SerializeField]
    private OrbGenerationPanel abilityOrbPanel = null;

    [SerializeField]
    private AbilityBonusTooltipSection bonusSectionPrefab = null;

    [SerializeField]
    private Color buffedNumericColour = default;

    [SerializeField]
    private Color deBuffedNumericColour = default;

    private PlayerTreeTooltipBuilder playerTreeTooltipBuilder;

    private List<AbilityBonusTooltipSection> bonusSections = new List<AbilityBonusTooltipSection>();

    public float TooltipHeightNoOrbs => descriptionText.preferredHeight + 36f;
    public float TooltipHeightWithOrbs => descriptionText.preferredHeight + 60f;

    private void Start()
    {
        Player player = RoomManager.Instance.Player;
        playerTreeTooltipBuilder = new PlayerTreeTooltipBuilder(player);
    }

    public void Deactivate() => DeactivateTooltip();

    /// <summary>
    /// Used to update the tooltip for abilities not in an ability tree.
    /// </summary>
    /// <param name="ability"></param>
    public void Activate(AbilityReference ability)
    {
        ActivateTooltip();

        string titleText = GenerateTitle(ability);

        bool isFinisher = AbilityLookup.Instance.IsFinisher(ability);

        List<TooltipSegment> segments = PlayerListTooltipBuilder.Build(ability);
        string descriptionText = GenerateDescription(segments);

        OrbCollection generatedOrbs = AbilityLookup.Instance.GetGeneratedOrbs(ability);

        List<AbilityBonusData> bonuses = AbilityLookup.Instance.GetAbilityBonuses(ability);

        SetContents(titleText, isFinisher, descriptionText, bonuses, generatedOrbs, l => PlayerListTooltipBuilder.Build(ability));
    }

    /// <summary>
    /// Used to update tooltip for abilities in the ability tree.
    /// </summary>
    /// <param name="node"></param>
    public void Activate(Node node)
    {
        ActivateTooltip();

        string titleText = GenerateTitle(node.Ability);

        bool isFinisher = AbilityLookup.Instance.IsFinisher(node.Ability);

        List<TooltipSegment> segments = playerTreeTooltipBuilder.Build(node);
        string descriptionText = GenerateDescription(segments);

        OrbCollection generatedOrbs = AbilityLookup.Instance.GetGeneratedOrbs(node.Ability);

        List<AbilityBonusData> bonuses = AbilityLookup.Instance.GetAbilityBonuses(node.Ability);

        SetContents(titleText, isFinisher, descriptionText, bonuses, generatedOrbs);
    }

    private string GenerateTitle(AbilityReference ability)
    {
        bool hasOrbType = AbilityLookup.Instance.TryGetAbilityOrbType(ability, out OrbType abilityOrbType);

        string title = AbilityLookup.Instance.GetAbilityDisplayName(ability);

        if (hasOrbType)
        {
            Color colour = OrbLookup.Instance.GetColour(abilityOrbType);
            title = TextUtils.ColouredText(colour, title);
        }

        return title;
    }

    private string GenerateDescription(List<TooltipSegment> segments)
    {
        string description = "";

        foreach (TooltipSegment segment in segments)
        {
            switch (segment.Type)
            {
                case TooltipSegmentType.Text:
                case TooltipSegmentType.UnaffectedNumericValue:
                    description += segment.Value;
                    break;

                case TooltipSegmentType.BuffedNumericValue:
                    description += $"{TextUtils.ColouredText(buffedNumericColour, segment.Value)}";
                    break;
                
                case TooltipSegmentType.DebuffedNumericValue:
                    description += $"{TextUtils.ColouredText(deBuffedNumericColour, segment.Value)}";
                    break;
            }
        }

        return description;
    }

    private void SetContents(string title, bool isFinisher, string description, List<AbilityBonusData> bonuses, OrbCollection orbCollection)
    {
        titleText.text = title;
        finisherText.enabled = isFinisher;
        descriptionText.text = description;
        abilityOrbPanel.DisplayOrbs(orbCollection);

        foreach (AbilityBonusTooltipSection section in bonusSections)
        {
            Destroy(section.gameObject);
        }

        bonusSections = new List<AbilityBonusTooltipSection>();

        foreach (AbilityBonusData bonus in bonuses)
        {
            AbilityBonusTooltipSection section = Instantiate(bonusSectionPrefab, Vector3.zero, Quaternion.identity, transform);
            section.Initialise(bonus.DisplayName, GenerateDescription(segmenter(bonus)));

            bonusSections.Add(section);
        }

        bool hasOrbs = !orbCollection.IsEmpty;

        tooltipPanel.sizeDelta = new Vector2(
            tooltipPanel.sizeDelta.x,
            0f
        );

        this.NextFrame(() => SetHeight(hasOrbs));
    }

    private void SetHeight(bool includeOrbs)
    {
        descriptionText.rectTransform.sizeDelta = new Vector2(
            descriptionText.rectTransform.sizeDelta.x,
            descriptionText.preferredHeight
        );

        abilityOrbPanel.transform.parent.gameObject.SetActive(includeOrbs);

        float newHeight = includeOrbs ? TooltipHeightWithOrbs : TooltipHeightNoOrbs;

        float bonusHeights = bonusSections.Select(s => s.GetSectionHeight()).Sum();

        tooltipPanel.sizeDelta = new Vector2(
            tooltipPanel.sizeDelta.x,
            newHeight + bonusHeights
        );
    }
}
