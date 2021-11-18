using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityTooltip : Tooltip
{
    [SerializeField]
    private Text titleText = null;

    [SerializeField]
    private Text descriptionText = null;

    [SerializeField]
    private SupplementaryTooltipPanel abilitySupplementaryTooltipPanel = null;
    
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

        string titleText = AbilityLookup.Instance.GetDisplayName(abilityId);
        Color color = RarityLookup.Instance.Lookup[AbilityLookup.Instance.GetRarity(abilityId)].Colour;
        string descriptionText = GenerateDescription(abilityId);

        SetContents(titleText, color, descriptionText);

        abilitySupplementaryTooltipPanel.Activate(abilityId);
    }

    private string GenerateDescription(SerializableGuid abilityId)
    {
        string description = "";

        AbilityType abilityType = AbilityLookup.Instance.GetAbilityType(abilityId);
        description += $"Type: {abilityType.ToString()}\n";
        
        int damage = AbilityLookup.Instance.GetDamage(abilityId);
        description += $"Damage: {damage.ToString()}";
        
        List<Empowerment> empowerments = AbilityLookup.Instance.GetEmpowerments(abilityId);
        if (empowerments.Count > 0)
        {
            description += "\nEmpowerments:";
            empowerments.ForEach(empowerment => description += $"\n    {empowerment.ToString()}");
        }

        return description;
    }

    private void SetContents(
        string title,
        Color color,
        string description
    )
    {
        titleText.text = title;
        titleText.color = color;
        descriptionText.text = description;
    }
}
