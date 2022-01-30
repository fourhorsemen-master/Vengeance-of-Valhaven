using System.Collections.Generic;
using UnityEngine;

public class AbilityGenerator
{
    private readonly RoomNode node;

    private readonly Dictionary<Zone, (float, float)> chanceRareByZone = new Dictionary<Zone, (float,float)>
    {
        [Zone.Zone1] = (0f, 0.25f),
        [Zone.Zone2] = (0.25f, 0.5f),
        [Zone.Zone3] = (0.5f, 0.75f)
    };

    private readonly Dictionary<Zone, (float, float)> chanceLegendaryByZone = new Dictionary<Zone, (float, float)>
    {
        [Zone.Zone1] = (0f, 0.05f),
        [Zone.Zone2] = (0.05f, 0.15f),
        [Zone.Zone3] = (0.15f, 0.3f)
    };

    private readonly Dictionary<Rarity, float> rarityDamageMultipliers = new Dictionary<Rarity, float>
    {
        [Rarity.Common] = 1f,
        [Rarity.Rare] = 1.4f,
        [Rarity.Legendary] = 1.8f,
    };

    public AbilityGenerator(RoomNode node)
    {
        this.node = node;
    }

    public Ability Generate()
    {
        AbilityType type = ChooseType();

        Rarity rarity = ChooseRarity();

        List<Empowerment> empowerments = new List<Empowerment>();

        int damage = (int) (AbilityTypeLookup.Instance.GetAbilityBaseDamage(type) * rarityDamageMultipliers[rarity]);

        string displayName = "Generated Ability";

        return new Ability(
            displayName,
            type,
            damage,
            rarity,
            empowerments
        );
    }

    private AbilityType ChooseType()
    {
        return RandomUtils.Choice(
            AbilityType.Lunge,
            AbilityType.Slash,
            AbilityType.Smash
        );
    }

    private Rarity ChooseRarity()
    {
        float zoneProgress = DepthUtils.GetDepthProportion(node);

        (float minChanceRare, float maxChanceRare) = chanceRareByZone[node.Zone];
        float chanceRare = Mathf.Lerp(minChanceRare, maxChanceRare, zoneProgress);

        (float minChanceLegendary, float maxChanceLegendary) = chanceLegendaryByZone[node.Zone];
        float chanceLegendary = Mathf.Lerp(minChanceLegendary, maxChanceLegendary, zoneProgress);

        var randomValue = Random.value;

        if (randomValue < chanceLegendary) return Rarity.Legendary;
        if (randomValue < chanceRare) return Rarity.Rare;
        return Rarity.Common;
    }
}

