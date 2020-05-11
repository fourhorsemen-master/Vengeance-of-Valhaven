using System;
using System.Collections.Generic;
using System.Linq;

public class AbilityMetadataLookup : Singleton<AbilityMetadataLookup>
{
    public SerializedMetadataLookup serializedMetadataLookup = new SerializedMetadataLookup();

    private Dictionary<AbilityReference, string> displayNameLookup = new Dictionary<AbilityReference, string>();
    private Dictionary<AbilityReference, string> tooltipLookup = new Dictionary<AbilityReference, string>();
    private Dictionary<AbilityReference, AbilityData> baseAbilityDataLookup = new Dictionary<AbilityReference, AbilityData>();
    private Dictionary<AbilityReference, OrbType?> abilityOrbTypeLookup = new Dictionary<AbilityReference, OrbType?>();
    private Dictionary<AbilityReference, Dictionary<OrbType, int>> generatedOrbsLookup = new Dictionary<AbilityReference, Dictionary<OrbType, int>>();

    protected override void Awake()
    {
        base.Awake();
        
        BuildLookups();
    }

    private void BuildLookups()
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
}
