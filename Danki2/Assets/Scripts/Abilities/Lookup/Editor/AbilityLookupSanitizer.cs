using System.Collections.Generic;
using System.Linq;

public class AbilityLookupSanitizer
{
    private readonly SerializableMetadataLookup serializableMetadataLookup;
    private readonly Dictionary<AbilityReference, AttributeData<AbilityAttribute>> abilityAttributeDataLookup;

    public AbilityLookupSanitizer(
        SerializableMetadataLookup serializableMetadataLookup,
        List<AttributeData<AbilityAttribute>> abilityAttributeData
    )
    {
        this.serializableMetadataLookup = serializableMetadataLookup;
        abilityAttributeDataLookup = abilityAttributeData.ToDictionary(
            d => d.Attribute.AbilityReference,
            d => d
        );
    }

    public void Sanitize()
    {
        RemoveUnnecessaryKeys();
        AddMissingKeys();
        foreach (AbilityReference abilityReference in abilityAttributeDataLookup.Keys)
        {
            SanitizeValue(abilityReference);
        }
    }

    private void RemoveUnnecessaryKeys()
    {
        foreach (AbilityReference abilityReference in serializableMetadataLookup.Keys)
        {
            if (!abilityAttributeDataLookup.ContainsKey(abilityReference))
            {
                serializableMetadataLookup.Remove(abilityReference);
            }
        }
    }

    private void AddMissingKeys()
    {
        foreach (AbilityReference abilityReference in abilityAttributeDataLookup.Keys)
        {
            if (!serializableMetadataLookup.ContainsKey(abilityReference))
            {
                serializableMetadataLookup.Add(abilityReference, new SerializableAbilityMetadata());
            }
        }
    }

    private void SanitizeValue(AbilityReference abilityReference)
    {
        SerializableAbilityBonusLookup serializableAbilityBonusLookup = serializableMetadataLookup[abilityReference].AbilityBonusLookup;
        string[] abilityBonuses = abilityAttributeDataLookup[abilityReference].Attribute.AbilityBonuses;

        RemoveUnnecessaryBonusKeys(serializableAbilityBonusLookup, abilityBonuses);
        AddMissingBonusKeys(serializableAbilityBonusLookup, abilityBonuses);
    }

    private void RemoveUnnecessaryBonusKeys(
        SerializableAbilityBonusLookup serializableAbilityBonusLookup,
        string[] abilityBonuses
    )
    {
        foreach (string key in serializableAbilityBonusLookup.Keys)
        {
            if (!abilityBonuses.Contains(key))
            {
                serializableAbilityBonusLookup.Remove(key);
            }
        }
    }

    private void AddMissingBonusKeys(
        SerializableAbilityBonusLookup serializableAbilityBonusLookup,
        string[] abilityBonuses
    )
    {
        foreach (string abilityBonus in abilityBonuses)
        {
            if (!serializableAbilityBonusLookup.ContainsKey(abilityBonus))
            {
                serializableAbilityBonusLookup.Add(abilityBonus, new SerializableAbilityBonusMetadata());
            }
        }
    }
}
