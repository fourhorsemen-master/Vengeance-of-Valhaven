using System.Collections.Generic;
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
    private SupplementaryTooltipPanel abilitySupplementaryTooltipPanel = null;

    private readonly List<AbilityBonusTooltipSection> bonusSections = new List<AbilityBonusTooltipSection>();
    
    public static AbilityTooltip Create(Transform transform, SerializableGuid abilityId)
    {
        AbilityTooltip abilityTooltip = Instantiate(TooltipLookup.Instance.AbilityTooltipPrefab, transform);
        abilityTooltip.Activate(abilityId);

        return abilityTooltip;
    }

    public static AbilityTooltip Create(Transform transform, Node node)
    {
        AbilityTooltip abilityTooltip = Instantiate(TooltipLookup.Instance.AbilityTooltipPrefab, transform);
        abilityTooltip.Activate(node.AbilityId);

        return abilityTooltip;
    }

    private void Activate(SerializableGuid abilityId)
    {
        ActivateTooltip();

        string titleText = AbilityLookup2.Instance.GetDisplayName(abilityId);
        Color color = RarityLookup.Instance.Lookup[AbilityLookup2.Instance.GetRarity(abilityId)].Colour;
        bool isFinisher = false;
        string descriptionText = GenerateDescription(abilityId);

        SetContents(titleText, color, isFinisher, descriptionText);

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

    private void SetContents(
        string title,
        Color color,
        bool isFinisher,
        string description
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
    }
}
