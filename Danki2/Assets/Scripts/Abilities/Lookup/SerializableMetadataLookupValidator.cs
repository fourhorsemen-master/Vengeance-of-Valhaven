using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SerializableMetadataLookupValidator
{
    private SerializableMetadataLookup serializableMetadataLookup;
    private List<AttributeData<AbilityAttribute>> abilityAttributeData;
    private Dictionary<AbilityReference, AttributeData<AbilityAttribute>> abilityAttributeDataLookup;
    
    private readonly Lexer lexer = new Lexer();
    private readonly TokenValidator tokenValidator = new TokenValidator();

    public bool HasErrors { get; private set; } = false;

    /// <summary>
    /// Performs validation on the given serializable metadata lookup. This checks that all of the metadata present
    /// aligns with the given attribute data and that there is no extra data that should not be there. Also, individual
    /// items are checked to make sure the required fields are set, for example, the display names should not be empty.
    /// This will log any errors found in the metadata and set the HasErrors flag accordingly.
    /// </summary>
    /// <param name="serializableMetadataLookup"> The metadata to validate. </param>
    /// <param name="abilityAttributeData"> The attribute data to validate against. </param>
    public void Validate(
        SerializableMetadataLookup serializableMetadataLookup,
        List<AttributeData<AbilityAttribute>> abilityAttributeData
    )
    {
        this.serializableMetadataLookup = serializableMetadataLookup;
        this.abilityAttributeData = abilityAttributeData;

        abilityAttributeDataLookup = this.abilityAttributeData.ToDictionary(
            d => d.Attribute.AbilityReference,
            d => d
        );

        HasErrors = false;
        
        ValidateAttributeUsage();
        ValidateSerializableAbilityMetadataLookup();
    }

    private void ValidateAttributeUsage()
    {
        List<AbilityReference> attributeAbilityReferences = abilityAttributeData
            .Select(d => d.Attribute.AbilityReference)
            .ToList();

        List<AbilityReference> duplicates = attributeAbilityReferences
            .GroupBy(r => r)
            .Where(g => g.Count() > 1)
            .Select(g => g.Key)
            .ToList();

        duplicates.ForEach(
            abilityReference => LogError($"Duplicate ability reference in attributes for {abilityReference.ToString()}")
        );

        if (attributeAbilityReferences.Distinct().Count() != Enum.GetValues(typeof(AbilityReference)).Length)
        {
            LogError("Ability attributes found do not match all ability references. There may be abilities missing the attribute.");
        }
        
        abilityAttributeData.ForEach(attributeData =>
        {
            Type type = attributeData.Type;
            
            if (!type.IsSubclassOf(typeof(InstantCast)) && !type.IsSubclassOf(typeof(Channel)))
            {
                LogError($"Found ability attribute on type \"{type}\", which does not inherit from a recognised ability class.");
            }

            if (type.GetConstructor(new [] {typeof(Actor), typeof(AbilityData), typeof(string[])}) == null)
            {
                LogError($"Could not find valid ability constructor on annotated type \"{type}\".");
            }
        });
    }

    private void ValidateSerializableAbilityMetadataLookup()
    {
        foreach (AbilityReference abilityReference in Enum.GetValues(typeof(AbilityReference)))
        {
            if (!serializableMetadataLookup.ContainsKey(abilityReference))
            {
                LogError($"No ability metadata found for {abilityReference.ToString()}, please add in the editor.");
                continue;
            }
            
            ValidateSerializableAbilityMetadata(abilityReference);
        }
    }

    private void ValidateSerializableAbilityMetadata(AbilityReference abilityReference)
    {
        SerializableAbilityMetadata serializableAbilityMetadata = serializableMetadataLookup[abilityReference];
        
        if (string.IsNullOrWhiteSpace(serializableAbilityMetadata.DisplayName))
        {
            LogError($"Null or white space display name for {abilityReference.ToString()}");
        }

        string tooltip = serializableAbilityMetadata.Tooltip;
        if (string.IsNullOrWhiteSpace(tooltip))
        {
            LogError($"Null or white space tooltip for {abilityReference.ToString()}");
        }
        else
        {
            List<Token> tokens = lexer.Lex(tooltip);
            if (!tokenValidator.HasValidSyntax(tokens))
            {
                LogError($"Tooltip for {abilityReference.ToString()} does not have valid syntax, value was: \"{tooltip}\".");
            }
        }

        ValidateSerializableAbilityBonusLookup(abilityReference);
    }

    private void ValidateSerializableAbilityBonusLookup(AbilityReference abilityReference)
    {
        SerializableAbilityBonusLookup serializableAbilityBonusLookup = serializableMetadataLookup[abilityReference].AbilityBonusLookup;
        string[] attributeAbilityBonuses = abilityAttributeDataLookup[abilityReference].Attribute.AbilityBonuses;
        string[] serializedAbilityBonuses = serializableAbilityBonusLookup.Keys.ToArray();
        
        foreach (string attributeAbilityBonus in attributeAbilityBonuses)
        {
            if (!serializedAbilityBonuses.Contains(attributeAbilityBonus))
            {
                LogError($"Serialized ability bonuses for {abilityReference.ToString()} does not contain entry for \"{attributeAbilityBonus}\".");
            }
        }
        
        foreach (string serializedAbilityBonus in serializedAbilityBonuses)
        {
            if (!attributeAbilityBonuses.Contains(serializedAbilityBonus))
            {
                LogError($"Invalid ability bonus \"{serializedAbilityBonus}\" found in serialized bonuses for {abilityReference.ToString()}.");
            }
        }
        
        foreach (string abilityBonus in serializedAbilityBonuses)
        {
            ValidateSerializableAbilityBonusMetadata(abilityReference, abilityBonus);
        }
    }

    private void ValidateSerializableAbilityBonusMetadata(AbilityReference abilityReference, string abilityBonus)
    {
        SerializableAbilityBonusMetadata serializableAbilityBonusMetadata = serializableMetadataLookup[abilityReference].AbilityBonusLookup[abilityBonus];

        if (string.IsNullOrWhiteSpace(serializableAbilityBonusMetadata.DisplayName))
        {
            LogError($"Null or white space display name found for ability bonus \"{abilityBonus}\" for {abilityReference.ToString()}.");
        }

        if (string.IsNullOrWhiteSpace(serializableAbilityBonusMetadata.Tooltip))
        {
            LogError($"Null or white space tooltip found for ability bonus \"{abilityBonus}\" for {abilityReference.ToString()}.");
        }

        if (serializableAbilityBonusMetadata.RequiredOrbs.Count == 0)
        {
            LogError($"No required orbs for ability bonus \"{abilityBonus}\" for {abilityReference.ToString()}.");
        }
    }
    
    private void LogError(string message)
    {
        Debug.LogError(message);
        HasErrors = true;
    }
}
