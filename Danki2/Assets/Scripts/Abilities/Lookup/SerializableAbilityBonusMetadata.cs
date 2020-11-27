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

    public string DisplayName { get => displayName; set => displayName = value; }
    public string Tooltip { get => tooltip; set => tooltip = value; }

    public bool MissingData => string.IsNullOrWhiteSpace(displayName) ||
                               string.IsNullOrWhiteSpace(tooltip);
}
