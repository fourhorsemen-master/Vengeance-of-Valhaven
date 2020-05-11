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
    private AbilityData baseBaseAbilityData = AbilityData.Zero;
    [SerializeField]
    private OrbType? abilityOrbType = null;
    [SerializeField]
    private List<OrbType> generatedOrbs = new List<OrbType>();

    public string DisplayName { get => displayName; set => displayName = value; }
    public string Tooltip { get => tooltip; set => tooltip = value; }
    public AbilityData BaseAbilityData { get => baseBaseAbilityData; set => baseBaseAbilityData = value; }
    public OrbType? AbilityOrbType { get => abilityOrbType; set => abilityOrbType = value; }
    public List<OrbType> GeneratedOrbs { get => generatedOrbs; set => generatedOrbs = value; }

    public bool Valid => !string.IsNullOrEmpty(displayName) && !string.IsNullOrEmpty(tooltip);
}
