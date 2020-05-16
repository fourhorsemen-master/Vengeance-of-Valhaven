using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SerializableAbilityMetadata
{
    [SerializeField]
    private string displayName = "";
    [SerializeField]
    private string tooltip = "";
    [SerializeField]
    private AbilityData baseAbilityData = AbilityData.Zero;
    [SerializeField]
    private SerializableNullableOrbType abilityOrbType = new SerializableNullableOrbType();
    [SerializeField]
    private List<OrbType> generatedOrbs = new List<OrbType>();
    [SerializeField]
    private SerializableAbilityBonusLookup abilityBonusLookup = new SerializableAbilityBonusLookup();

    public string DisplayName { get => displayName; set => displayName = value; }
    public string Tooltip { get => tooltip; set => tooltip = value; }
    public AbilityData BaseAbilityData { get => baseAbilityData; set => baseAbilityData = value; }
    public SerializableNullableOrbType AbilityOrbType { get => abilityOrbType; set => abilityOrbType = value; }
    public List<OrbType> GeneratedOrbs { get => generatedOrbs; set => generatedOrbs = value; }
    public SerializableAbilityBonusLookup AbilityBonusLookup { get => abilityBonusLookup; set => abilityBonusLookup = value; }

    public bool Valid => !string.IsNullOrEmpty(displayName) &&
                         !string.IsNullOrEmpty(tooltip) &&
                         abilityBonusLookup.Valid;
}
