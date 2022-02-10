using System.Collections.Generic;
using UnityEngine;

public class AbilityGenerator
{
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

    private readonly Zone zone;
    private readonly float depthProportion;

    public AbilityGenerator(Zone zone, float depthProportion)
    {
        this.zone = zone;
        this.depthProportion = depthProportion;
    }

    public Ability Generate()
    {
        AbilityType type = ChooseType();

        Rarity rarity = ChooseRarity();

        Augmentation augmentation = RandomUtils.Choice(RarityLookup.Instance.Lookup[rarity].Augmentations);

        int damage = (int) (AbilityTypeLookup.Instance.GetAbilityBaseDamage(type) * rarityDamageMultipliers[rarity]);

        string displayName = $"{augmentation.Descriptor} {type}";

        return new Ability(
            displayName,
            type,
            damage,
            rarity,
            augmentation.Empowerments
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
        (float minChanceRare, float maxChanceRare) = chanceRareByZone[zone];
        float chanceRare = Mathf.Lerp(minChanceRare, maxChanceRare, depthProportion);

        (float minChanceLegendary, float maxChanceLegendary) = chanceLegendaryByZone[zone];
        float chanceLegendary = Mathf.Lerp(minChanceLegendary, maxChanceLegendary, depthProportion);

        float randomValue = Random.value;

        if (randomValue < chanceLegendary) return Rarity.Legendary;
        if (randomValue < chanceRare) return Rarity.Rare;
        return Rarity.Common;
    }
}

