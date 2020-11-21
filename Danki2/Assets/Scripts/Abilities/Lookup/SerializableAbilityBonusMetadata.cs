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
    [SerializeField]
    private int requiredTreeDepth = 0;

    public string DisplayName { get => displayName; set => displayName = value; }
    public string Tooltip { get => tooltip; set => tooltip = value; }
    public List<OrbType> RequiredOrbs { get => requiredOrbs; set => requiredOrbs = value; }
    public int RequiredTreeDepth { get => requiredTreeDepth; set => requiredTreeDepth = value; }

    public bool MissingData => string.IsNullOrWhiteSpace(displayName) ||
                               string.IsNullOrWhiteSpace(tooltip) ||
                               requiredOrbs.Count == 0;
}
