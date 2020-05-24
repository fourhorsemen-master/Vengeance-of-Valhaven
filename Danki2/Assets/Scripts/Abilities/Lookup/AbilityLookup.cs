using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class AbilityLookup : Singleton<AbilityLookup>
{
    public SerializableMetadataLookup serializableMetadataLookup = new SerializableMetadataLookup();

    private readonly AbilityMap<string> displayNameMap = new AbilityMap<string>();
    private readonly AbilityMap<OrbType> abilityOrbTypeMap = new AbilityMap<OrbType>();
    private readonly AbilityMap<AbilityData> baseAbilityDataMap = new AbilityMap<AbilityData>();
    private readonly AbilityMap<OrbCollection> generatedOrbsMap = new AbilityMap<OrbCollection>();
    private readonly AbilityMap<List<TemplatedTooltipSegment>> templatedTooltipSegmentsMap = new AbilityMap<List<TemplatedTooltipSegment>>();
    private readonly AbilityMap<Dictionary<string, AbilityBonusData>> abilityBonusDataMap = new AbilityMap<Dictionary<string, AbilityBonusData>>();

    private readonly AbilityMap<Func<Actor, AbilityData, string[], InstantCast>> instantCastBuilderMap = new AbilityMap<Func<Actor, AbilityData, string[], InstantCast>>();
    private readonly AbilityMap<Func<Actor, AbilityData, string[], Channel>> channelBuilderMap = new AbilityMap<Func<Actor, AbilityData, string[], Channel>>();
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

    public bool TryGetInstantCast(
        AbilityReference abilityReference,
        Actor owner,
        AbilityData abilityDataDiff,
        string[] activeBonuses,
        out InstantCast ability
    )
    {
        if (instantCastBuilderMap.ContainsKey(abilityReference))
        {
            AbilityData abilityData = baseAbilityDataMap[abilityReference] + abilityDataDiff;
            ability = instantCastBuilderMap[abilityReference](owner, abilityData, activeBonuses);
            return true;
        }

        ability = null;
        return false;
    }

    public bool TryGetChannel(
        AbilityReference abilityReference,
        Actor owner,
        AbilityData abilityDataDiff,
        string[] activeBonuses,
        out Channel ability
    )
    {
        if (channelBuilderMap.ContainsKey(abilityReference))
        {
            AbilityData abilityData = baseAbilityDataMap[abilityReference] + abilityDataDiff;
            ability = channelBuilderMap[abilityReference](owner, abilityData, activeBonuses);
            return true;
        }

        ability = null;
        return false;
    }

    public bool TryGetAbilityOrbType(AbilityReference abilityReference, out OrbType orbType)
    {
        if (abilityOrbTypeMap.ContainsKey(abilityReference))
        {
            orbType = abilityOrbTypeMap[abilityReference];
            return true;
        }

        orbType = default;
        return false;
    }

    public AbilityType GetAbilityType(AbilityReference abilityReference) => abilityTypeMap[abilityReference];

    public AbilityData GetBaseAbilityData(AbilityReference abilityReference) => baseAbilityDataMap[abilityReference];

    public OrbCollection GetGeneratedOrbs(AbilityReference abilityReference) => generatedOrbsMap[abilityReference];

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
            if (serializableAbilityMetadata.AbilityOrbType.HasValue)
            {
                abilityOrbTypeMap[abilityReference] = serializableAbilityMetadata.AbilityOrbType.Value;
            }
            generatedOrbsMap[abilityReference] = new OrbCollection(serializableAbilityMetadata.GeneratedOrbs);
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
            ConstructorInfo constructor = type.GetConstructor(new [] {typeof(Actor), typeof(AbilityData), typeof(string[])});

            if (type.IsSubclassOf(typeof(InstantCast)))
            {
                instantCastBuilderMap[abilityReference] = (a, b, c) => (InstantCast)constructor.Invoke(new object[] {a, b, c});
                abilityTypeMap[abilityReference] = AbilityType.InstantCast;
                continue;
            }

            if (type.IsSubclassOf(typeof(Channel)))
            {
                channelBuilderMap[abilityReference] = (a, b, c) => (Channel)constructor.Invoke(new object[] {a, b, c});
                abilityTypeMap[abilityReference] = AbilityType.Channel;
            }
        }
    }
}
