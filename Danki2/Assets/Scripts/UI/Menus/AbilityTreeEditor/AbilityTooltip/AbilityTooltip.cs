using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityTooltip : Tooltip<AbilityTooltip>
{
    private const string FinisherText = "Finisher";
    
    [SerializeField]
    private Text title = null;

    [SerializeField]
    private Text finisher = null;

    [SerializeField]
    private Text description = null;

    [SerializeField]
    private OrbGenerationPanel abilityOrbPanel = null;

    [SerializeField]
    private Color buffedNumericColour = default;

    [SerializeField]
    private Color deBuffedNumericColour = default;

    private PlayerTreeTooltipBuilder playerTreeTooltipBuilder;

    private bool heightInitialised = false;
    public float TooltipHeightNoOrbs => description.preferredHeight + 36f;
    public float TooltipHeightWithOrbs => description.preferredHeight + 60f;

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

        string finisherText = GenerateFinisherText(ability);

        List<TooltipSegment> segments = PlayerListTooltipBuilder.Build(ability);
        string descriptionText = GenerateDescription(segments);

        OrbCollection generatedOrbs = AbilityLookup.Instance.GetGeneratedOrbs(ability);

        SetContents(titleText, finisherText, descriptionText, generatedOrbs);
    }

    /// <summary>
    /// Used to update tooltip for abilities in the ability tree.
    /// </summary>
    /// <param name="node"></param>
    public void Activate(Node node)
    {
        ActivateTooltip();

        string titleText = GenerateTitle(node.Ability);

        string finisherText = GenerateFinisherText(node.Ability);

        List<TooltipSegment> segments = playerTreeTooltipBuilder.Build(node);
        string descriptionText = GenerateDescription(segments);

        OrbCollection generatedOrbs = AbilityLookup.Instance.GetGeneratedOrbs(node.Ability);

        SetContents(titleText, finisherText, descriptionText, generatedOrbs);
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

    private string GenerateFinisherText(AbilityReference abilityReference)
    {
        if (!AbilityLookup.Instance.IsFinisher(abilityReference))
        {
            return "";
        }

        return AbilityLookup.Instance.TryGetAbilityOrbType(abilityReference, out OrbType abilityOrbType)
            ? TextUtils.ColouredText(OrbLookup.Instance.GetColour(abilityOrbType), FinisherText)
            : FinisherText;
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

    private void SetContents(string titleText, string finisherText, string descriptionText, OrbCollection orbCollection)
    {
        title.text = titleText;
        finisher.text = finisherText;
        description.text = descriptionText;
        abilityOrbPanel.DisplayOrbs(orbCollection);

        bool hasOrbs = !orbCollection.IsEmpty;

        if (!heightInitialised)
        {
            this.NextFrame(() => SetHeight(hasOrbs));
            heightInitialised = true;
        }
        else
        {
            SetHeight(hasOrbs);
        }
    }

    private void SetHeight(bool includeOrbs)
    {
        description.rectTransform.sizeDelta = new Vector2(
            description.rectTransform.sizeDelta.x,
            description.preferredHeight
        );

        float newHeight = includeOrbs ? TooltipHeightWithOrbs : TooltipHeightNoOrbs;

        tooltipPanel.sizeDelta = new Vector2(
            tooltipPanel.sizeDelta.x,
            newHeight
        );
    }
}
