using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class AbilityMetadataLookup : Singleton<AbilityMetadataLookup>
{
    public SerializedMetadataLookup serializedMetadataLookup = new SerializedMetadataLookup();

    private readonly Dictionary<AbilityReference, string> displayNameLookup =
        new Dictionary<AbilityReference, string>();
    private readonly Dictionary<AbilityReference, string> tooltipLookup =
        new Dictionary<AbilityReference, string>();
    private readonly Dictionary<AbilityReference, AbilityData> baseAbilityDataLookup =
        new Dictionary<AbilityReference, AbilityData>();
    private readonly Dictionary<AbilityReference, OrbType?> abilityOrbTypeLookup =
        new Dictionary<AbilityReference, OrbType?>();
    private readonly Dictionary<AbilityReference, Dictionary<OrbType, int>> generatedOrbsLookup =
        new Dictionary<AbilityReference, Dictionary<OrbType, int>>();

    private readonly Dictionary<AbilityReference, Func<Actor, AbilityData, InstantCast>> instantCastBuilderLookup =
        new Dictionary<AbilityReference, Func<Actor, AbilityData, InstantCast>>();
    private readonly Dictionary<AbilityReference, Func<Actor, AbilityData, Channel>> channelBuilderLookup =
        new Dictionary<AbilityReference, Func<Actor, AbilityData, Channel>>();
    private readonly Dictionary<AbilityReference, AbilityType> abilityTypeLookup =
        new Dictionary<AbilityReference, AbilityType>();

    protected override void Awake()
    {
        base.Awake();
        
        BuildMetadataLookups();
        BuildAbilityBuilderLookups();
    }

    public bool TryGetInstantCast(
        AbilityReference abilityReference,
        Actor owner,
        AbilityData abilityDataDiff,
        out InstantCast ability
    )
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

    public bool TryGetChannel(
        AbilityReference abilityReference,
        Actor owner,
        AbilityData abilityDataDiff,
        out Channel ability
    )
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

    public string GetAbilityTooltip(AbilityReference abilityReference)
    {
        return tooltipLookup[abilityReference];
    }

    public string GetAbilityDisplayName(AbilityReference abilityReference)
    {
        return displayNameLookup[abilityReference];
    }

    private void BuildMetadataLookups()
    {
        foreach (AbilityReference abilityReference in Enum.GetValues(typeof(AbilityReference)))
        {
            SerializableAbilityMetadata abilityMetadata = serializedMetadataLookup[abilityReference];
            displayNameLookup[abilityReference] = abilityMetadata.DisplayName;
            tooltipLookup[abilityReference] = abilityMetadata.Tooltip;
            baseAbilityDataLookup[abilityReference] = abilityMetadata.BaseAbilityData;
            abilityOrbTypeLookup[abilityReference] = abilityMetadata.AbilityOrbType.HasValue
                ? abilityMetadata.AbilityOrbType.Value
                : (OrbType?)null;
            generatedOrbsLookup[abilityReference] = abilityMetadata.GeneratedOrbs
                .GroupBy(o => o)
                .ToDictionary(g => g.Key, g => g.Count());
        }
    }

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
                Debug.Log($"Could not find valid constructor for ability: {abilityReference}");
                return;
            }

            if (type.IsSubclassOf(typeof(InstantCast)))
            {
                instantCastBuilderLookup[abilityReference] = (a, b) => (InstantCast)constructor.Invoke(new object[] {a, b});
                abilityTypeLookup[abilityReference] = AbilityType.InstantCast;
            }

            if (type.IsSubclassOf(typeof(Channel)))
            {
                channelBuilderLookup[abilityReference] = (a, b) => (Channel)constructor.Invoke(new object[] {a, b});
                abilityTypeLookup[abilityReference] = AbilityType.Channel;
            }
        }
    }

    private bool IsValidAbilityAttributeData(List<AttributeData<AbilityAttribute>> abilityAttributeData)
    {
        List<AbilityReference> abilityReferences = abilityAttributeData
            .Select(d => d.Attribute.AbilityReference)
            .ToList();

        if (abilityReferences.Distinct().Count() != abilityReferences.Count)
        {
            Debug.LogError("Ability attributes are not distinct.");
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
