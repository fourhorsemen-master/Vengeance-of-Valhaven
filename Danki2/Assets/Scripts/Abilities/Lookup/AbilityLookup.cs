﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class AbilityLookup : Singleton<AbilityLookup>
{
    public SerializableMetadataLookup serializableMetadataLookup = new SerializableMetadataLookup();

    private readonly Dictionary<AbilityReference, string> displayNameLookup =
        new Dictionary<AbilityReference, string>();
    private readonly Dictionary<AbilityReference, List<TemplatedTooltipSegment>> templatedTooltipSegmentsLookup =
        new Dictionary<AbilityReference, List<TemplatedTooltipSegment>>();
    private readonly Dictionary<AbilityReference, AbilityData> baseAbilityDataLookup =
        new Dictionary<AbilityReference, AbilityData>();
    private readonly Dictionary<AbilityReference, OrbType?> abilityOrbTypeLookup =
        new Dictionary<AbilityReference, OrbType?>();
    private readonly Dictionary<AbilityReference, Dictionary<OrbType, int>> generatedOrbsLookup =
        new Dictionary<AbilityReference, Dictionary<OrbType, int>>();
    private readonly Dictionary<AbilityReference, Dictionary<string, AbilityBonusData>> abilityBonusDataLookup =
        new Dictionary<AbilityReference, Dictionary<string, AbilityBonusData>>();

    private readonly Dictionary<AbilityReference, Func<Actor, AbilityData, InstantCast>> instantCastBuilderLookup =
        new Dictionary<AbilityReference, Func<Actor, AbilityData, InstantCast>>();
    private readonly Dictionary<AbilityReference, Func<Actor, AbilityData, Channel>> channelBuilderLookup =
        new Dictionary<AbilityReference, Func<Actor, AbilityData, Channel>>();
    private readonly Dictionary<AbilityReference, AbilityType> abilityTypeLookup =
        new Dictionary<AbilityReference, AbilityType>();

    private readonly Lexer lexer = new Lexer();
    private readonly TokenValidator tokenValidator = new TokenValidator();
    private readonly Parser parser = new Parser();

    private List<AttributeData<AbilityAttribute>> abilityAttributeData;
    private Dictionary<AbilityReference, AttributeData<AbilityAttribute>> abilityAttributeDataLookup;

    protected override void Awake()
    {
        base.Awake();

        abilityAttributeData = ReflectionUtils.GetAttributeData<AbilityAttribute>(Assembly.GetExecutingAssembly());
        abilityAttributeDataLookup = abilityAttributeData.ToDictionary(
            d => d.Attribute.AbilityReference,
            d => d
        );

        BuildMetadataLookups();
        BuildAbilityBuilderLookups();
    }

    public bool TryGetInstantCast(AbilityReference abilityReference, Actor owner, AbilityData abilityDataDiff, out InstantCast ability)
    {
        if (instantCastBuilderLookup.ContainsKey(abilityReference))
        {
            AbilityData abilityData = baseAbilityDataLookup[abilityReference] + abilityDataDiff;
            ability = instantCastBuilderLookup[abilityReference](owner, abilityData);
            return true;
        }

        ability = null;
        return false;
    }

    public bool TryGetChannel(AbilityReference abilityReference, Actor owner, AbilityData abilityDataDiff, out Channel ability)
    {
        if (channelBuilderLookup.ContainsKey(abilityReference))
        {
            AbilityData abilityData = baseAbilityDataLookup[abilityReference] + abilityDataDiff;
            ability = channelBuilderLookup[abilityReference](owner, abilityData);
            return true;
        }

        ability = null;
        return false;
    }

    public AbilityType GetAbilityType(AbilityReference abilityReference)
    {
        return abilityTypeLookup[abilityReference];
    }

    public AbilityData GetBaseAbilityData(AbilityReference abilityReference)
    {
        return baseAbilityDataLookup[abilityReference];
    }

    public Dictionary<OrbType, int> GetGeneratedOrbs(AbilityReference abilityReference)
    {
        return generatedOrbsLookup[abilityReference];
    }

    public OrbType? GetAbilityOrbType(AbilityReference abilityReference)
    {
        return abilityOrbTypeLookup[abilityReference];
    }

    public List<TemplatedTooltipSegment> GetTemplatedTooltipSegments(AbilityReference abilityReference)
    {
        return templatedTooltipSegmentsLookup[abilityReference];
    }

    public string GetAbilityDisplayName(AbilityReference abilityReference)
    {
        return displayNameLookup[abilityReference];
    }

    public Dictionary<string, AbilityBonusData> GetAbilityBonusDataLookup(AbilityReference abilityReference)
    {
        return abilityBonusDataLookup[abilityReference];
    }

    private void BuildMetadataLookups()
    {
        if (!HasValidSerializableMetadataLookup()) return;

        foreach (AbilityReference abilityReference in Enum.GetValues(typeof(AbilityReference)))
        {
            SerializableAbilityMetadata abilityMetadata = serializableMetadataLookup[abilityReference];

            displayNameLookup[abilityReference] = abilityMetadata.DisplayName;
            baseAbilityDataLookup[abilityReference] = abilityMetadata.BaseAbilityData;
            abilityOrbTypeLookup[abilityReference] = abilityMetadata.AbilityOrbType.HasValue
                ? abilityMetadata.AbilityOrbType.Value
                : (OrbType?)null;
            generatedOrbsLookup[abilityReference] = abilityMetadata.GeneratedOrbs
                .GroupBy(o => o)
                .ToDictionary(g => g.Key, g => g.Count());
            BuildTooltip(abilityReference, abilityMetadata.Tooltip);
            BuildAbilityBonusLookup(abilityReference, abilityMetadata.AbilityBonusLookup);
        }
    }

    private bool HasValidSerializableMetadataLookup()
    {
        foreach (AbilityReference abilityReference in Enum.GetValues(typeof(AbilityReference)))
        {
            if (!serializableMetadataLookup.ContainsKey(abilityReference))
            {
                Debug.LogError($"No ability metadata found for {abilityReference.ToString()}, please add in the editor.");
                return false;
            }

            if (!serializableMetadataLookup[abilityReference].Valid)
            {
                Debug.LogError($"Invalid ability metadata for {abilityReference.ToString()}, please change in the editor.");
                return false;
            }
        }

        return true;
    }

    private void BuildTooltip(AbilityReference abilityReference, string tooltip)
    {
        List<Token> tokens = lexer.Lex(tooltip);

        if (!tokenValidator.HasValidSyntax(tokens))
        {
            Debug.LogError($"Tooltip for {abilityReference.ToString()} does not have valid syntax, value was: \"{tooltip}\".");
            return;
        }

        templatedTooltipSegmentsLookup[abilityReference] = parser.Parse(tokens);
    }

    private void BuildAbilityBonusLookup(
        AbilityReference abilityReference,
        SerializableAbilityBonusLookup serializableAbilityBonusLookup
    )
    {
        if (!serializableAbilityBonusLookup.Valid)
        {
            Debug.LogError($"Invalid ability bonus data for {abilityReference.ToString()}, please change in the editor.");
            return;
        }

        string[] attributeAbilityBonuses = abilityAttributeDataLookup[abilityReference].Attribute.AbilityBonuses;
        string[] serializedAbilityBonuses = serializableAbilityBonusLookup.Keys.ToArray();

        if (attributeAbilityBonuses.Length != serializedAbilityBonuses.Length)
        {
            Debug.LogError("Attribute ability bonuses do not match serialized ability bonuses in length");
            return;
        }

        for (int i = 0; i < attributeAbilityBonuses.Length; i++)
        {
            string attributeAbilityBonus = attributeAbilityBonuses[i];
            string serializedAbilityBonus = serializedAbilityBonuses[i];
            if (attributeAbilityBonus != serializedAbilityBonus)
            {
                Debug.LogError($"Attribute ability bonus \"{attributeAbilityBonus}\" does not match serialized ability bonus {serializedAbilityBonus}");
                return;
            }
        }

        abilityBonusDataLookup[abilityReference] = attributeAbilityBonuses.ToDictionary(
            abilityBonus => abilityBonus,
            abilityBonus =>
            {
                SerializableAbilityBonusMetadata serializableAbilityBonusMetadata = serializableAbilityBonusLookup[abilityBonus];
                return new AbilityBonusData(
                    serializableAbilityBonusMetadata.DisplayName,
                    serializableAbilityBonusMetadata.Tooltip,
                    new OrbCollection(serializableAbilityBonusMetadata.RequiredOrbs)
                );
            }
        );
    }

    /// <summary>
    /// Checks for classes with the ability attribute, validates that they are on the correct class and that we have
    /// the expected number of annotations, then gets the constructors manually for these classes and adds them to
    /// the ability builder lookups.
    /// </summary>
    private void BuildAbilityBuilderLookups()
    {
        if (!IsValidAbilityAttributeData()) return;

        Dictionary<AbilityReference, Type> abilityReferenceToType = abilityAttributeData
            .ToDictionary(d => d.Attribute.AbilityReference, d => d.Type);

        foreach (AbilityReference abilityReference in Enum.GetValues(typeof(AbilityReference)))
        {
            Type type = abilityReferenceToType[abilityReference];
            ConstructorInfo constructor = type.GetConstructor(new [] {typeof(Actor), typeof(AbilityData)});

            if (constructor == null)
            {
                Debug.Log($"Could not find valid constructor for ability: {abilityReference}.");
                return;
            }

            if (type.IsSubclassOf(typeof(InstantCast)))
            {
                instantCastBuilderLookup[abilityReference] = (a, b) => (InstantCast)constructor.Invoke(new object[] {a, b});
                abilityTypeLookup[abilityReference] = AbilityType.InstantCast;
                continue;
            }

            if (type.IsSubclassOf(typeof(Channel)))
            {
                channelBuilderLookup[abilityReference] = (a, b) => (Channel)constructor.Invoke(new object[] {a, b});
                abilityTypeLookup[abilityReference] = AbilityType.Channel;
                continue;
            }

            Debug.LogError($"Ability {abilityReference} does not inherit from a recognised ability class.");
        }
    }

    private bool IsValidAbilityAttributeData()
    {
        List<AbilityReference> abilityReferences = abilityAttributeData
            .Select(d => d.Attribute.AbilityReference)
            .ToList();

        if (abilityReferences.Distinct().Count() != abilityReferences.Count)
        {
            Debug.LogError("Ability attributes do not contain distinct ability references.");
            return false;
        }

        if (abilityReferences.Count != Enum.GetValues(typeof(AbilityReference)).Length)
        {
            Debug.LogError("Ability attributes found do not match all AbilityReferences. There may be abilities missing the attribute.");
            return false;
        }

        if (!abilityAttributeData.All(d => d.Type.IsSubclassOf(typeof(Ability))))
        {
            Debug.LogError("Found ability attribute on class that does not inherit from ability.");
            return false;
        }

        return true;
    }
}
