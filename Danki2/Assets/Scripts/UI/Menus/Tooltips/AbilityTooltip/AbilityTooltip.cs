using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityTooltip : Tooltip
{
    [SerializeField]
    private Text titleText = null;

    [SerializeField]
    private Text damageText = null;

    [SerializeField]
    private AbilityTooltipEmpowerment abilityTooltipEmpowermentPrefab = null;

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

        string titleText = AbilityLookup.Instance.GetDisplayName(abilityId);
        Color color = RarityLookup.Instance.Lookup[AbilityLookup.Instance.GetRarity(abilityId)].Colour;
        string damageText = $"Damage: {AbilityLookup.Instance.GetDamage(abilityId)}";
        List<Empowerment> empowerments = AbilityLookup.Instance.GetEmpowerments(abilityId);

        SetContents(titleText, color, damageText, empowerments);

        ActivateSupplementaryTooltips(abilityId);
    }

    private void SetContents(
        string title,
        Color color,
        string description,
        List<Empowerment> empowerments
    )
    {
        titleText.text = title;
        titleText.color = color;
        damageText.text = description;

        empowerments.ForEach(e =>
        {
            Instantiate(abilityTooltipEmpowermentPrefab, tooltipPanel)
                .SetEmpowerment(e);
        });

        foreach (AbilityBonusTooltipSection section in bonusSections)
        {
            Destroy(section.gameObject);
        }

        bonusSections.Clear();
    }

    private void ActivateSupplementaryTooltips(SerializableGuid abilityId)
    {
        List<Empowerment> empowerments = AbilityLookup.Instance.GetEmpowerments(abilityId);

        List<ActiveEffect> activeEffects = new List<ActiveEffect>();
        List<PassiveEffect> passiveEffects = new List<PassiveEffect>();
        List<StackingEffect> stackingEffects = new List<StackingEffect>();
        
        empowerments.ForEach(e =>
        {
            activeEffects.AddRange(EmpowermentLookup.Instance.GetActiveEffects(e));
            passiveEffects.AddRange(EmpowermentLookup.Instance.GetPassiveEffects(e));
            stackingEffects.AddRange(EmpowermentLookup.Instance.GetStackingEffects(e));
        });
        
        abilitySupplementaryTooltipPanel.Activate(
            empowerments,
            activeEffects,
            passiveEffects,
            stackingEffects
        );
    }
}
