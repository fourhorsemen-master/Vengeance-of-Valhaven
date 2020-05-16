using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SerializableAbilityBonusMetadata
{
    [SerializeField]
    private string displayName = "";
    [SerializeField]
    private string tooltip = "";
    [SerializeField]
    private List<OrbType> requiredOrbs = new List<OrbType>();

    public string DisplayName { get => displayName; set => displayName = value; }
    public string Tooltip { get => tooltip; set => tooltip = value; }
    public List<OrbType> RequiredOrbs { get => requiredOrbs; set => requiredOrbs = value; }

    public bool Valid => !string.IsNullOrEmpty(displayName) &&
                         !string.IsNullOrEmpty(tooltip) &&
                         requiredOrbs.Count != 0;
}
