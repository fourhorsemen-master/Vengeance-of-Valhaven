using System;
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
    private SerializableAbilityBonusLookup abilityBonusLookup = new SerializableAbilityBonusLookup();
    [SerializeField]
    private bool finisher = false;
    [SerializeField]
    private float channelDuration = 0f;

    public string DisplayName { get => displayName; set => displayName = value; }
    public string Tooltip { get => tooltip; set => tooltip = value; }
    public AbilityData BaseAbilityData { get => baseAbilityData; set => baseAbilityData = value; }
    public SerializableAbilityBonusLookup AbilityBonusLookup { get => abilityBonusLookup; set => abilityBonusLookup = value; }
    public bool Finisher { get => finisher; set => finisher = value; }
    public float ChannelDuration { get => channelDuration; set => channelDuration = value; }

    public bool MissingData => string.IsNullOrWhiteSpace(displayName) ||
                               string.IsNullOrWhiteSpace(tooltip) ||
                               abilityBonusLookup.MissingData;
}
