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
    private AbilityData abilityData = AbilityData.Zero;
    [SerializeField]
    private List<OrbType> generatedOrbs = new List<OrbType>();

    public string DisplayName { get => displayName; set => displayName = value; }
    public string Tooltip { get => tooltip; set => tooltip = value; }
    public AbilityData AbilityData { get => abilityData; set => abilityData = value; }
    public List<OrbType> GeneratedOrbs { get => generatedOrbs; set => generatedOrbs = value; }
}
