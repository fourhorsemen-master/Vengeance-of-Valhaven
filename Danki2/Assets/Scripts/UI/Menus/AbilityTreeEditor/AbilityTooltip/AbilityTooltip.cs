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
    private OrbGenerationPanel abilityOrbPanel = null;

    private PlayerTooltipBuilder tooltipBuilder;

    public float TooltipHeightNoOrbs => description.preferredHeight + 36f;
    public float TooltipHeightWithOrbs => description.preferredHeight + 60f;

    private void Start()
    {
        gameObject.SetActive(false);

        Player player = RoomManager.Instance.Player;
        tooltipBuilder = new PlayerTooltipBuilder(player);
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
        title.text = AbilityLookup.Instance.GetAbilityDisplayName(node.Ability);

        description.text = GenerateDescription(node);

        description.rectTransform.sizeDelta = new Vector2(
            description.rectTransform.sizeDelta.x,
            description.preferredHeight
        );

        OrbCollection generatedOrbs = AbilityLookup.Instance.GetGeneratedOrbs(node.Ability);
        abilityOrbPanel.DisplayOrbs(generatedOrbs);

        float newHeight = generatedOrbs.IsEmpty
            ? TooltipHeightNoOrbs
            : TooltipHeightWithOrbs;

        tooltipPanel.sizeDelta = new Vector2(
            tooltipPanel.sizeDelta.x,
            newHeight
        );
    }

    private string GenerateDescription(Node node)
    {
        List<TooltipSegment> segments = tooltipBuilder.Build(node);

        bool hasOrbType = AbilityLookup.Instance.TryGetAbilityOrbType(node.Ability, out OrbType abilityOrbType);

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

    private void MoveToMouse()
    {
        tooltipPanel.position = Input.mousePosition;
    }
}
