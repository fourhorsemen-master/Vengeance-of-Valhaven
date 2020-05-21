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
    private readonly AbilityMap<Dictionary<string, AbilityBonusData>> abilityBonusDataMap = new AbilityMap<Dictionary<string, AbilityBonusData>>();

    private readonly AbilityMap<Func<Actor, AbilityData, InstantCast>> instantCastBuilderMap = new AbilityMap<Func<Actor, AbilityData, InstantCast>>();
    private readonly AbilityMap<Func<Actor, AbilityData, Channel>> channelBuilderMap = new AbilityMap<Func<Actor, AbilityData, Channel>>();
    private readonly AbilityMap<AbilityType> abilityTypeMap = new AbilityMap<AbilityType>();

    private readonly Lexer lexer = new Lexer();
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

        SerializableMetadataLookupValidator serializableMetadataLookupValidator = new SerializableMetadataLookupValidator();
        serializableMetadataLookupValidator.Validate(serializableMetadataLookup, abilityAttributeData);
        if (serializableMetadataLookupValidator.HasErrors)
        {
            Debug.LogError("Serializable metadata lookup errors found, see above for errors. Aborting building of ability lookups.");
            return;
        }

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

    public Dictionary<string, AbilityBonusData> GetAbilityBonusDataLookup(AbilityReference abilityReference) => abilityBonusDataMap[abilityReference];

    private void BuildMetadataLookups()
    {
        foreach (AbilityReference abilityReference in Enum.GetValues(typeof(AbilityReference)))
        {
            SerializableAbilityMetadata serializableAbilityMetadata = serializableMetadataLookup[abilityReference];

            displayNameMap[abilityReference] = serializableAbilityMetadata.DisplayName;
            baseAbilityDataMap[abilityReference] = serializableAbilityMetadata.BaseAbilityData;
            abilityOrbTypeMap[abilityReference] = serializableAbilityMetadata.AbilityOrbType.HasValue
                ? serializableAbilityMetadata.AbilityOrbType.Value
                : (OrbType?)null;
            generatedOrbsMap[abilityReference] = serializableAbilityMetadata.GeneratedOrbs
                .GroupBy(o => o)
                .ToDictionary(g => g.Key, g => g.Count());
            BuildTooltip(abilityReference, serializableAbilityMetadata.Tooltip);
            BuildAbilityBonusLookup(abilityReference, serializableAbilityMetadata.AbilityBonusLookup);
        }
    }

    private void BuildTooltip(AbilityReference abilityReference, string tooltip)
    {
        List<Token> tokens = lexer.Lex(tooltip);
        templatedTooltipSegmentsMap[abilityReference] = parser.Parse(tokens);
    }

    private void BuildAbilityBonusLookup(
        AbilityReference abilityReference,
        SerializableAbilityBonusLookup serializableAbilityBonusLookup
    )
    {
        string[] abilityBonuses = abilityAttributeDataLookup[abilityReference].Attribute.AbilityBonuses;

        abilityBonusDataMap[abilityReference] = abilityBonuses.ToDictionary(
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
    /// Checks for classes with the ability attribute and gets the constructors manually for these classes and adds them
    /// to the ability builder lookups.
    /// </summary>
    private void BuildAbilityBuilderLookups()
    {
        Dictionary<AbilityReference, Type> abilityReferenceToType = abilityAttributeData
            .ToDictionary(d => d.Attribute.AbilityReference, d => d.Type);

        foreach (AbilityReference abilityReference in Enum.GetValues(typeof(AbilityReference)))
        {
            Type type = abilityReferenceToType[abilityReference];
            ConstructorInfo constructor = type.GetConstructor(new [] {typeof(Actor), typeof(AbilityData)});

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
            }
        }
    }
}
