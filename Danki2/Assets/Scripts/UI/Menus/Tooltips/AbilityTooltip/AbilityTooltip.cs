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
    
    public static AbilityTooltip Create(Transform transform, Ability ability)
    {
        AbilityTooltip abilityTooltip = Instantiate(TooltipLookup.Instance.AbilityTooltipPrefab, transform);
        abilityTooltip.Activate(ability);

        return abilityTooltip;
    }

    public static AbilityTooltip Create(Transform transform, Node node)
    {
        AbilityTooltip abilityTooltip = Instantiate(TooltipLookup.Instance.AbilityTooltipPrefab, transform);
        abilityTooltip.Activate(node.Ability);

        return abilityTooltip;
    }

    private void Activate(Ability ability)
    {
        ActivateTooltip();

        Color color = RarityLookup.Instance.Lookup[ability.Rarity].Colour;
        string damageText = $"Damage: {ability.Damage}";

        SetContents(ability.DisplayName, color, damageText, ability.Empowerments);

        ActivateSupplementaryTooltips(ability);
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

        foreach (Empowerment empowerment in empowerments)
        {
            Instantiate(abilityTooltipEmpowermentPrefab, tooltipPanel)
                .SetEmpowerment(empowerment);
        }

        foreach (AbilityBonusTooltipSection section in bonusSections)
        {
            Destroy(section.gameObject);
        }

        bonusSections.Clear();
    }

    private void ActivateSupplementaryTooltips(Ability ability)
    {
        List<Empowerment> empowerments = ability.Empowerments;

        List<ActiveEffect> activeEffects = new List<ActiveEffect>();
        List<PassiveEffect> passiveEffects = new List<PassiveEffect>();
        List<StackingEffect> stackingEffects = new List<StackingEffect>();

        foreach (Empowerment empowerment in empowerments)
        {
            activeEffects.AddRange(EmpowermentLookup.Instance.GetActiveEffects(empowerment));
            passiveEffects.AddRange(EmpowermentLookup.Instance.GetPassiveEffects(empowerment));
            stackingEffects.AddRange(EmpowermentLookup.Instance.GetStackingEffects(empowerment));
        }
        
        abilitySupplementaryTooltipPanel.Activate(
            empowerments,
            activeEffects,
            passiveEffects,
            stackingEffects
        );
    }
}
