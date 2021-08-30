using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class AbilityTooltip : Tooltip
{
    [SerializeField]
    private Text titleText = null;

    [SerializeField]
    private Text finisherText = null;

    [SerializeField]
    private Text descriptionText = null;

    [SerializeField]
    private AbilityBonusTooltipSection bonusSectionPrefab = null;

    [SerializeField]
    private Color buffedNumericColour = default;

    [SerializeField]
    private Color deBuffedNumericColour = default;

    [SerializeField]
    private SupplementaryTooltipPanel abilitySupplementaryTooltipPanel = null;

    [SerializeField]
    private RectTransform tooltipRectTransform = null;

    private readonly List<AbilityBonusTooltipSection> bonusSections = new List<AbilityBonusTooltipSection>();

    private PlayerTreeTooltipBuilder PlayerTreeTooltipBuilder => new PlayerTreeTooltipBuilder(ActorCache.Instance.Player);
    
    public static AbilityTooltip Create(Transform transform, SerializableGuid abilityId)
    {
        AbilityTooltip abilityTooltip = Instantiate(TooltipLookup.Instance.AbilityTooltipPrefab, transform);
        abilityTooltip.Activate(
            abilityId,
            PlayerListTooltipBuilder.Build(abilityId),
            bonus => PlayerListTooltipBuilder.BuildBonus(abilityId, bonus)
        );

        return abilityTooltip;
    }

    public static AbilityTooltip Create(Transform transform, Node node)
    {
        AbilityTooltip abilityTooltip = Instantiate(TooltipLookup.Instance.AbilityTooltipPrefab, transform);
        abilityTooltip.Activate(
            node.AbilityId,
            abilityTooltip.PlayerTreeTooltipBuilder.Build(node),
            bonus => abilityTooltip.PlayerTreeTooltipBuilder.BuildBonus(node, bonus),
            node.Depth
        );

        return abilityTooltip;
    }

    private void Activate(
        SerializableGuid abilityId,
        List<TooltipSegment> tooltipSegments,
        Func<string, List<TooltipSegment>> bonusSegmenter,
        int? treeDepth = null
    )
    {
        ActivateTooltip();

        string titleText = AbilityLookup2.Instance.GetDisplayName(abilityId);
        Color color = RarityLookup.Instance.Lookup[AbilityLookup2.Instance.GetRarity(abilityId)].Colour;
        // bool isFinisher = AbilityLookup.Instance.IsFinisher(ability);
        bool isFinisher = false;
        string descriptionText = GenerateDescription(abilityId);

        // Dictionary<string, AbilityBonusData> bonuses = AbilityLookup.Instance.GetAbilityBonusDataLookup(ability);
        Dictionary<string, AbilityBonusData> bonuses = new Dictionary<string, AbilityBonusData>();

        SetContents(titleText, color, isFinisher, descriptionText, bonuses, bonusSegmenter, treeDepth);

        abilitySupplementaryTooltipPanel.Activate(abilityId);
    }

    private string GenerateDescription(SerializableGuid abilityId)
    {
        string description = "";

        AbilityType2 abilityType = AbilityLookup2.Instance.GetAbilityType(abilityId);
        description += $"Type: {abilityType.ToString()}\n";
        
        int damage = AbilityLookup2.Instance.GetDamage(abilityId);
        description += $"Damage: {damage.ToString()}\n";
        
        List<Empowerment> empowerments = AbilityLookup2.Instance.GetEmpowerments(abilityId);
        if (empowerments.Count > 0)
        {
            description += "Empowerments:\n";
            empowerments.ForEach(empowerment => description += $"    {empowerment.ToString()}");
        }

        return description;
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

                case TooltipSegmentType.BoldText:
                    description += TextUtils.BoldText(segment.Value);
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

    private void SetContents(
        string title,
        Color color,
        bool isFinisher,
        string description,
        Dictionary<string, AbilityBonusData> bonuses,
        Func<string, List<TooltipSegment>> segmenter,
        int? treeDepth = null
    )
    {
        titleText.text = title;
        titleText.color = color;
        finisherText.enabled = isFinisher;
        descriptionText.text = description;

        foreach (AbilityBonusTooltipSection section in bonusSections)
        {
            Destroy(section.gameObject);
        }

        bonusSections.Clear();

        List<string> bonusKeys = bonuses.Keys.ToList();
        bonusKeys.Sort((bonus1, bonus2) => bonuses[bonus1].RequiredTreeDepth.CompareTo(bonuses[bonus2].RequiredTreeDepth));
        
        bonusKeys.ForEach(bonus =>
        {
            AbilityBonusData bonusData = bonuses[bonus];
            AbilityBonusTooltipSection section = Instantiate(bonusSectionPrefab, Vector3.zero, Quaternion.identity, tooltipRectTransform);
            section.Initialise(
                bonusData.DisplayName,
                GenerateDescription(segmenter(bonus)),
                bonusData.RequiredTreeDepth,
                !treeDepth.HasValue || bonusData.RequiredTreeDepth <= treeDepth.Value
            );

            bonusSections.Add(section);
        });
    }
}
