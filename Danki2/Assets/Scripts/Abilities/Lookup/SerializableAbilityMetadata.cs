using FMODUnity;
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

    [SerializeField, EventRef]
    private string fmodStartEventRef = "";
    [SerializeField, EventRef]
    private string fmodEndEventRef = "";

    [SerializeField]
    AbilityAnimationType animationType = AbilityAnimationType.None;

    [SerializeField]
    private SerializableAbilityBonusLookup abilityBonusLookup = new SerializableAbilityBonusLookup();
    [SerializeField]
    private bool playerCanCast = true;
    [SerializeField]
    private bool finisher = false;
    [SerializeField]
    private float channelDuration = 0f;

    public string DisplayName { get => displayName; set => displayName = value; }
    public string Tooltip { get => tooltip; set => tooltip = value; }
    public AbilityData BaseAbilityData { get => baseAbilityData; set => baseAbilityData = value; }
    public string FmodStartEventRef { get => fmodStartEventRef; set => fmodStartEventRef = value; }
    public string FmodEndEventRef { get => fmodEndEventRef; set => fmodEndEventRef = value; }
    public AbilityAnimationType AnimationType { get => animationType; set => animationType = value; }
    public SerializableAbilityBonusLookup AbilityBonusLookup { get => abilityBonusLookup; set => abilityBonusLookup = value; }
    public bool PlayerCanCast { get => playerCanCast; set => playerCanCast = value; }
    public bool Finisher { get => finisher; set => finisher = value; }
    public float ChannelDuration { get => channelDuration; set => channelDuration = value; }

    public bool MissingData => string.IsNullOrWhiteSpace(displayName) ||
                               string.IsNullOrWhiteSpace(tooltip) ||
                               abilityBonusLookup.MissingData;
}
