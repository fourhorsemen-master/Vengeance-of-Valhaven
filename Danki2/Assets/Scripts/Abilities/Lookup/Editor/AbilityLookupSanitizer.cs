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
        List<AbilityReference> keysToRemove = new List<AbilityReference>();
        
        foreach (AbilityReference abilityReference in serializableMetadataLookup.Keys)
        {
            if (!abilityAttributeDataLookup.ContainsKey(abilityReference))
            {
                keysToRemove.Add(abilityReference);
            }
        }
        
        keysToRemove.ForEach(k => serializableMetadataLookup.Remove(k));
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
        List<string> keysToRemove = new List<string>();
        
        foreach (string key in serializableAbilityBonusLookup.Keys)
        {
            if (!abilityBonuses.Contains(key))
            {
                keysToRemove.Add(key);
            }
        }
        
        keysToRemove.ForEach(k => serializableAbilityBonusLookup.Remove(k));
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
