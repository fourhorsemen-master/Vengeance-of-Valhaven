using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class AbilityLookup : Singleton<AbilityLookup>
{
    public SerializableMetadataLookup serializableMetadataLookup = new SerializableMetadataLookup();

    private readonly AbilityMap<string> displayNameMap = new AbilityMap<string>();
    private readonly AbilityMap<OrbType?> abilityOrbTypeMap = new AbilityMap<OrbType?>();
    private readonly AbilityMap<AbilityData> baseAbilityDataMap = new AbilityMap<AbilityData>();
    private readonly AbilityMap<Dictionary<OrbType, int>> generatedOrbsMap = new AbilityMap<Dictionary<OrbType, int>>();
    private readonly AbilityMap<List<TemplatedTooltipSegment>> templatedTooltipSegmentsMap = new AbilityMap<List<TemplatedTooltipSegment>>();

    private readonly AbilityMap<Func<Actor, AbilityData, InstantCast>> instantCastBuilderMap = new AbilityMap<Func<Actor, AbilityData, InstantCast>>();
    private readonly AbilityMap<Func<Actor, AbilityData, Channel>> channelBuilderMap = new AbilityMap<Func<Actor, AbilityData, Channel>>();
    private readonly AbilityMap<AbilityType> abilityTypeMap = new AbilityMap<AbilityType>();

    private readonly Lexer lexer = new Lexer();
    private readonly TokenValidator tokenValidator = new TokenValidator();
    private readonly Parser parser = new Parser();

    protected override void Awake()
    {
        base.Awake();
        BuildMetadataLookups();
        BuildAbilityBuilderLookups();
    }

    public bool TryGetInstantCast(AbilityReference abilityReference, Actor owner, AbilityData abilityDataDiff, out InstantCast ability)
    {
        if (instantCastBuilderMap.ContainsKey(abilityReference))
        {
            AbilityData abilityData = baseAbilityDataMap[abilityReference] + abilityDataDiff;
            ability = instantCastBuilderMap[abilityReference](owner, abilityData);
            return true;
        }

        ability = null;
        return false;
    }

    public bool TryGetChannel(AbilityReference abilityReference, Actor owner, AbilityData abilityDataDiff, out Channel ability)
    {
        if (channelBuilderMap.ContainsKey(abilityReference))
        {
            AbilityData abilityData = baseAbilityDataMap[abilityReference] + abilityDataDiff;
            ability = channelBuilderMap[abilityReference](owner, abilityData);
            return true;
        }

        ability = null;
        return false;
    }

    public AbilityType GetAbilityType(AbilityReference abilityReference) => abilityTypeMap[abilityReference];

    public AbilityData GetBaseAbilityData(AbilityReference abilityReference) => baseAbilityDataMap[abilityReference];

    public Dictionary<OrbType, int> GetGeneratedOrbs(AbilityReference abilityReference) => generatedOrbsMap[abilityReference];

    public OrbType? GetAbilityOrbType(AbilityReference abilityReference) => abilityOrbTypeMap[abilityReference];

    public List<TemplatedTooltipSegment> GetTemplatedTooltipSegments(AbilityReference abilityReference) => templatedTooltipSegmentsMap[abilityReference];

    public string GetAbilityDisplayName(AbilityReference abilityReference) => displayNameMap[abilityReference];

    private void BuildMetadataLookups()
    {
        if (!HasValidSerializableMetadataLookup()) return;

        foreach (AbilityReference abilityReference in Enum.GetValues(typeof(AbilityReference)))
        {
            SerializableAbilityMetadata abilityMetadata = serializableMetadataLookup[abilityReference];

            displayNameMap[abilityReference] = abilityMetadata.DisplayName;
            baseAbilityDataMap[abilityReference] = abilityMetadata.BaseAbilityData;
            abilityOrbTypeMap[abilityReference] = abilityMetadata.AbilityOrbType.HasValue
                ? abilityMetadata.AbilityOrbType.Value
                : (OrbType?)null;
            generatedOrbsMap[abilityReference] = abilityMetadata.GeneratedOrbs
                .GroupBy(o => o)
                .ToDictionary(g => g.Key, g => g.Count());
            BuildTooltip(abilityReference, abilityMetadata.Tooltip);
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

        templatedTooltipSegmentsMap[abilityReference] = parser.Parse(tokens);
    }

    /// <summary>
    /// Checks for classes with the ability attribute, validates that they are on the correct class and that we have
    /// the expected number of annotations, then gets the constructors manually for these classes and adds them to
    /// the ability builder lookups.
    /// </summary>
    private void BuildAbilityBuilderLookups()
    {
        List<AttributeData<AbilityAttribute>> abilityAttributeData = ReflectionUtils
            .GetAttributeData<AbilityAttribute>(Assembly.GetExecutingAssembly());

        if (!IsValidAbilityAttributeData(abilityAttributeData)) return;

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
                instantCastBuilderMap[abilityReference] = (a, b) => (InstantCast)constructor.Invoke(new object[] {a, b});
                abilityTypeMap[abilityReference] = AbilityType.InstantCast;
                continue;
            }

            if (type.IsSubclassOf(typeof(Channel)))
            {
                channelBuilderMap[abilityReference] = (a, b) => (Channel)constructor.Invoke(new object[] {a, b});
                abilityTypeMap[abilityReference] = AbilityType.Channel;
                continue;
            }

            Debug.LogError($"Ability {abilityReference} does not inherit from a recognised ability class.");
        }
    }

    private bool IsValidAbilityAttributeData(List<AttributeData<AbilityAttribute>> abilityAttributeData)
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
