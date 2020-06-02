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
    private OrbGenerationPanel abilityOrbPanel = null;

    private PlayerTreeTooltipBuilder playerTreeTooltipBuilder;

    public float TooltipHeightNoOrbs => description.preferredHeight + 36f;
    public float TooltipHeightWithOrbs => description.preferredHeight + 60f;

    private float TooltipPanelWidth => tooltipPanel.sizeDelta.x * tooltipPanel.GetParentCanvas().scaleFactor;

    private void Start()
    {
        Player player = RoomManager.Instance.Player;
        playerTreeTooltipBuilder = new PlayerTreeTooltipBuilder(player);
    }

    private void Update()
    {
        if (gameObject.activeInHierarchy)
        {
            MoveToMouse();
        }
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

    /// <summary>
    /// Used to update the tooltip for abilities not in an ability tree.
    /// </summary>
    /// <param name="ability"></param>
    public void UpdateTooltip(AbilityReference ability)
    {
        string titleText = AbilityLookup.Instance.GetAbilityDisplayName(ability);

        List<TooltipSegment> segments = PlayerListTooltipBuilder.Build(ability);
        string descriptionText = GenerateDescription(segments, ability);
        
        SetContents(titleText, descriptionText);
    }

    /// <summary>
    /// Used to update tooltip for abilities in the ability tree.
    /// </summary>
    /// <param name="node"></param>
    public void UpdateTooltip(Node node)
    {
        string titleText = AbilityLookup.Instance.GetAbilityDisplayName(node.Ability);

        List<TooltipSegment> segments = playerTreeTooltipBuilder.Build(node);
        string descriptionText = GenerateDescription(segments, node.Ability);

        OrbCollection generatedOrbs = AbilityLookup.Instance.GetGeneratedOrbs(node.Ability);

        SetContents(titleText, descriptionText, generatedOrbs);
    }

    private string GenerateDescription(List<TooltipSegment> segments, AbilityReference ability)
    {
        bool hasOrbType = AbilityLookup.Instance.TryGetAbilityOrbType(ability, out OrbType abilityOrbType);

        string description = "";

        foreach (TooltipSegment segment in segments)
        {
            switch (segment.Type)
            {
                case TooltipSegmentType.Text:
                case TooltipSegmentType.BaseNumericValue:
                    description += segment.Value;
                    break;

                case TooltipSegmentType.BonusNumericValue:
                    if (!hasOrbType)
                    {
                        Debug.LogError("Bonus segment encountered on tooltip for ability without orb type.");
                        description += segment.Value;
                        break;
                    }

                    string bonus = $"+{segment.Value}";
                    Color colour = OrbLookup.Instance.GetColour(abilityOrbType);
                    string bonusWithColour = TextUtils.ColouredText(colour, bonus);

                    description += $" ({bonusWithColour})";
                    break;
            }
        }

        return description;
    }

    private void SetContents(string titleText, string descriptionText, OrbCollection orbs = null)
    {
        title.text = titleText;
        description.text = descriptionText;

        abilityOrbPanel.DisplayOrbs(orbs);

        description.rectTransform.sizeDelta = new Vector2(
            description.rectTransform.sizeDelta.x,
            description.preferredHeight
        );

        bool hasOrbs = orbs != null && !orbs.IsEmpty;
        float newHeight = hasOrbs ? TooltipHeightNoOrbs : TooltipHeightWithOrbs;

        tooltipPanel.sizeDelta = new Vector2(
            tooltipPanel.sizeDelta.x,
            newHeight
        );

    }

    private void MoveToMouse()
    {
        Vector3 newPosition = Input.mousePosition;

        float overlap = Mathf.Max(0f, newPosition.x + TooltipPanelWidth - Screen.width);

        newPosition.x -= overlap;

        tooltipPanel.position = newPosition;
    }
}
