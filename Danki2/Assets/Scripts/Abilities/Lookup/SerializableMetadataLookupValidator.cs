using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class SerializableMetadataLookupValidator
{
    private readonly SerializableMetadataLookup serializableMetadataLookup;
    private readonly List<AttributeData<AbilityAttribute>> abilityAttributeData;
    private readonly Dictionary<AbilityReference, AttributeData<AbilityAttribute>> abilityAttributeDataLookup;

    private readonly List<string> errors = new List<string>();
    
    private readonly Lexer lexer = new Lexer();
    private readonly TokenValidator tokenValidator = new TokenValidator();

    public bool HasErrors => errors.Count > 0;

    public SerializableMetadataLookupValidator(
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

        Validate();
    }

    public void LogErrors()
    {
        errors.ForEach(Debug.LogError);
    }

    private void Validate()
    {
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
            abilityReference => errors.Add($"Duplicate ability reference in attributes for {abilityReference.ToString()}")
        );

        if (attributeAbilityReferences.Distinct().Count() != Enum.GetValues(typeof(AbilityReference)).Length)
        {
            errors.Add("Ability attributes found do not match all ability references. There may be abilities missing the attribute.");
        }
        
        abilityAttributeData.ForEach(attributeData =>
        {
            Type type = attributeData.Type;
            
            if (!type.IsSubclassOf(typeof(InstantCast)) && !type.IsSubclassOf(typeof(Channel)))
            {
                errors.Add($"Found ability attribute on type \"{type}\", which does not inherit from a recognised ability class.");
            }

            ConstructorInfo constructor = type.GetConstructor(new [] {typeof(Actor), typeof(AbilityData)});

            if (constructor == null)
            {
                errors.Add($"Could not find valid ability constructor on annotated type \"{type}\".");
            }
        });
    }

    private void ValidateSerializableAbilityMetadataLookup()
    {
        foreach (AbilityReference abilityReference in Enum.GetValues(typeof(AbilityReference)))
        {
            if (!serializableMetadataLookup.ContainsKey(abilityReference))
            {
                errors.Add($"No ability metadata found for {abilityReference.ToString()}, please add in the editor.");
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
            errors.Add($"Null or white space display name for {abilityReference.ToString()}");
        }

        string tooltip = serializableAbilityMetadata.Tooltip;
        if (string.IsNullOrWhiteSpace(tooltip))
        {
            errors.Add($"Null or white space tooltip for {abilityReference.ToString()}");
        }
        else
        {
            List<Token> tokens = lexer.Lex(tooltip);
            if (!tokenValidator.HasValidSyntax(tokens))
            {
                errors.Add($"Tooltip for {abilityReference.ToString()} does not have valid syntax, value was: \"{tooltip}\".");
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
                errors.Add($"Serialized ability bonuses for {abilityReference.ToString()} does not contain entry for \"{attributeAbilityBonus}\".");
            }
        }
        
        foreach (string serializedAbilityBonus in serializedAbilityBonuses)
        {
            if (!attributeAbilityBonuses.Contains(serializedAbilityBonus))
            {
                errors.Add($"Invalid ability bonus \"{serializedAbilityBonus}\" found in serialized bonuses for {abilityReference.ToString()}.");
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
            errors.Add($"Null or white space display name found for ability bonus \"{abilityBonus}\" for {abilityReference.ToString()}.");
        }

        if (string.IsNullOrWhiteSpace(serializableAbilityBonusMetadata.Tooltip))
        {
            errors.Add($"Null or white space tooltip found for ability bonus \"{abilityBonus}\" for {abilityReference.ToString()}.");
        }

        if (serializableAbilityBonusMetadata.RequiredOrbs.Count == 0)
        {
            errors.Add($"No required orbs for ability bonus \"{abilityBonus}\" for {abilityReference.ToString()}.");
        }
    }
}
