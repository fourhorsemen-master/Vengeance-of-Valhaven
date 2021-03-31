using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class AbilityLookup : Singleton<AbilityLookup>
{
    [SerializeField]
    public SerializableMetadataLookup serializableMetadataLookup = new SerializableMetadataLookup(
        () => new SerializableAbilityMetadata()
    );

    private readonly AbilityMap<string> displayNameMap = new AbilityMap<string>();
    private readonly AbilityMap<AbilityData> baseAbilityDataMap = new AbilityMap<AbilityData>();
    private readonly AbilityMap<List<TemplatedTooltipSegment>> templatedTooltipSegmentsMap = new AbilityMap<List<TemplatedTooltipSegment>>();
    private readonly AbilityMap<Dictionary<string, AbilityBonusData>> abilityBonusDataMap = new AbilityMap<Dictionary<string, AbilityBonusData>>();
    private readonly AbilityMap<Rarity> rarityLookup = new AbilityMap<Rarity>();
    private readonly AbilityMap<bool> playerCanCastLookup = new AbilityMap<bool>();
    private readonly AbilityMap<bool> finisherLookup = new AbilityMap<bool>();
    private readonly AbilityMap<float> channelDurationMap = new AbilityMap<float>();
    private readonly AbilityMap<string> fmodStartEventRefs = new AbilityMap<string>();
    private readonly AbilityMap<string> fmodEndEventRefs = new AbilityMap<string>();
    private readonly AbilityMap<AbilityAnimationType> animationTypes = new AbilityMap<AbilityAnimationType>();

    private readonly AbilityMap<Func<AbilityConstructionArgs, InstantCast>> instantCastBuilderMap = new AbilityMap<Func<AbilityConstructionArgs, InstantCast>>();
    private readonly AbilityMap<Func<AbilityConstructionArgs, Channel>> channelBuilderMap = new AbilityMap<Func<AbilityConstructionArgs, Channel>>();
    private readonly AbilityMap<AbilityType> abilityTypeMap = new AbilityMap<AbilityType>();
    private readonly AbilityMap<ChannelType> channelTypeMap = new AbilityMap<ChannelType>();

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

            AbilityConstructionArgs args = new AbilityConstructionArgs(
                owner,
                abilityData,
                fmodStartEventRefs[abilityReference],
                fmodEndEventRefs[abilityReference],
                activeBonuses,
                animationTypes[abilityReference]
            );

            ability = instantCastBuilderMap[abilityReference](args);
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
        if (channelBuilderMap.ContainsKey(abilityReference) && channelDurationMap.ContainsKey(abilityReference))
        {
            AbilityData abilityData = baseAbilityDataMap[abilityReference] + abilityDataDiff;

            AbilityConstructionArgs args = new AbilityConstructionArgs(
                owner, 
                abilityData, 
                fmodStartEventRefs[abilityReference], 
                fmodEndEventRefs[abilityReference],
                activeBonuses, 
                animationTypes[abilityReference],
                channelDurationMap[abilityReference]
            );

            ability = channelBuilderMap[abilityReference](args);
            return true;
        }

        ability = null;
        return false;
    }

    public AbilityType GetAbilityType(AbilityReference abilityReference) => abilityTypeMap[abilityReference];

    public bool TryGetChannelType(AbilityReference abilityReference, out ChannelType channelType) =>
        channelTypeMap.TryGetValue(abilityReference, out channelType);

    public AbilityData GetBaseAbilityData(AbilityReference abilityReference) => baseAbilityDataMap[abilityReference];

    public List<TemplatedTooltipSegment> GetTemplatedTooltipSegments(AbilityReference abilityReference) => templatedTooltipSegmentsMap[abilityReference];

    public string GetAbilityDisplayName(AbilityReference abilityReference) => displayNameMap[abilityReference];

    public Dictionary<string, AbilityBonusData> GetAbilityBonusDataLookup(AbilityReference abilityReference) => abilityBonusDataMap[abilityReference];

    public Rarity GetRarity(AbilityReference abilityReference) => rarityLookup[abilityReference];

    public bool PlayerCanCast(AbilityReference abilityReference) => playerCanCastLookup[abilityReference];

    public bool IsFinisher(AbilityReference abilityReference) => finisherLookup[abilityReference];

    public bool TryGetChannelDuration(AbilityReference abilityReference, out float channelDuration) =>
        channelDurationMap.TryGetValue(abilityReference, out channelDuration);

    private void BuildMetadataLookups()
    {
        EnumUtils.ForEach<AbilityReference>(ability =>
        {
            SerializableAbilityMetadata serializableAbilityMetadata = serializableMetadataLookup[ability];

            displayNameMap[ability] = serializableAbilityMetadata.DisplayName;
            fmodStartEventRefs[ability] = serializableAbilityMetadata.FmodStartEventRef;
            fmodEndEventRefs[ability] = serializableAbilityMetadata.FmodEndEventRef;
            baseAbilityDataMap[ability] = serializableAbilityMetadata.BaseAbilityData;
            finisherLookup[ability] = serializableAbilityMetadata.Finisher;
            playerCanCastLookup[ability] = serializableAbilityMetadata.PlayerCanCast;
            rarityLookup[ability] = serializableAbilityMetadata.Rarity;
            animationTypes[ability] = serializableAbilityMetadata.AnimationType;

            if (abilityAttributeDataLookup[ability].Type.IsSubclassOf(typeof(Channel)))
            {
                channelDurationMap[ability] = serializableAbilityMetadata.ChannelDuration;
            }

            templatedTooltipSegmentsMap[ability] = BuildTooltip(serializableAbilityMetadata.Tooltip);
            BuildAbilityBonusLookup(ability, serializableAbilityMetadata.AbilityBonusLookup);
        });
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
                    BuildTooltip(serializableAbilityBonusMetadata.Tooltip),
                    serializableAbilityBonusMetadata.RequiredTreeDepth
                );
            }
        );
    }

    private List<TemplatedTooltipSegment> BuildTooltip(string tooltip)
    {
        List<Token> tokens = lexer.Lex(tooltip);
        return parser.Parse(tokens);
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

            if (type.IsSubclassOf(typeof(InstantCast)))
            {
                ConstructorInfo constructor = type.GetConstructor(new [] {typeof(AbilityConstructionArgs)});
                instantCastBuilderMap[abilityReference] = (a) => (InstantCast)constructor.Invoke(new object[] {a});
                abilityTypeMap[abilityReference] = AbilityType.InstantCast;
                continue;
            }

            if (type.IsSubclassOf(typeof(Channel)))
            {
                ConstructorInfo constructor = type.GetConstructor(new [] {typeof(AbilityConstructionArgs)});
                channelBuilderMap[abilityReference] = (a) => (Channel)constructor.Invoke(new object[] {a});
                abilityTypeMap[abilityReference] = AbilityType.Channel;
                BuildChannelTypeLookup(abilityReference, type);
            }
        }
    }

    private void BuildChannelTypeLookup(AbilityReference abilityReference, Type type)
    {
        if (type.IsSubclassOf(typeof(Cast)))
        {
            channelTypeMap[abilityReference] = ChannelType.Cast;
            return;
        }

        if (type.IsSubclassOf(typeof(Charge)))
        {
            channelTypeMap[abilityReference] = ChannelType.Charge;
            return;
        }

        if (type.IsSubclassOf(typeof(Channel)))
        {
            channelTypeMap[abilityReference] = ChannelType.Channel;
        }
    }
}
