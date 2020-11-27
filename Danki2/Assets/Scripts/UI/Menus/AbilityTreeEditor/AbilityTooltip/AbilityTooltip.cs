﻿using System;
using System.Collections.Generic;
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
    private AbilityBonusTooltipSection bonusSectionPrefab = null;

    [SerializeField]
    private Color buffedNumericColour = default;

    [SerializeField]
    private Color deBuffedNumericColour = default;

    private PlayerTreeTooltipBuilder playerTreeTooltipBuilder;

    private readonly List<AbilityBonusTooltipSection> bonusSections = new List<AbilityBonusTooltipSection>();

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
        List<TooltipSegment> tooltipSegments = PlayerListTooltipBuilder.Build(ability);

        Activate(
            ability,
            tooltipSegments,
            bonus => PlayerListTooltipBuilder.BuildBonus(ability, bonus)
        );
    }

    /// <summary>
    /// Used to update tooltip for abilities in the ability tree.
    /// </summary>
    /// <param name="node"></param>
    public void Activate(Node node)
    {
        List<TooltipSegment> tooltipSegments = playerTreeTooltipBuilder.Build(node);

        Activate(
            node.Ability,
            tooltipSegments,
            bonus => playerTreeTooltipBuilder.BuildBonus(node, bonus)
        );
    }

    private void Activate(AbilityReference ability, List<TooltipSegment> tooltipSegments, Func<string, List<TooltipSegment>> bonusSegmenter)
    {
        ActivateTooltip();

        string titleText = AbilityLookup.Instance.GetAbilityDisplayName(ability);
        bool isFinisher = AbilityLookup.Instance.IsFinisher(ability);
        string descriptionText = GenerateDescription(tooltipSegments);

        Dictionary<string, AbilityBonusData> bonuses = AbilityLookup.Instance.GetAbilityBonusDataLookup(ability);

        SetContents(titleText, isFinisher, descriptionText, bonuses, bonusSegmenter);
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

    private void SetContents(
        string title,
        bool isFinisher,
        string description,
        Dictionary<string, AbilityBonusData> bonuses,
        Func<string, List<TooltipSegment>> segmenter
    )
    {
        titleText.text = title;
        finisherText.enabled = isFinisher;
        descriptionText.text = description;

        foreach (AbilityBonusTooltipSection section in bonusSections)
        {
            Destroy(section.gameObject);
        }

        bonusSections.Clear();

        foreach (string bonus in bonuses.Keys)
        {
            AbilityBonusData bonusData = bonuses[bonus];
            AbilityBonusTooltipSection section = Instantiate(bonusSectionPrefab, Vector3.zero, Quaternion.identity, transform);
            section.Initialise(bonusData.DisplayName, GenerateDescription(segmenter(bonus)));

            bonusSections.Add(section);
        }
    }
}
