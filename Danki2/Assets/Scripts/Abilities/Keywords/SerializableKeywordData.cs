using System;
using UnityEngine;

[Serializable]
public class SerializableKeywordData
{
    [SerializeField] private string displayName = "";
    [SerializeField] private string description = "";

    public string DisplayName { get => displayName; set => displayName = value; }
    public string Description { get => description; set => description = value; }
}
