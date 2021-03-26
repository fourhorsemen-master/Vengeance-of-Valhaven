using System;
using UnityEngine;

[Serializable]
public class RarityData
{
    [SerializeField] private string displayName = "";
    [SerializeField] private Color colour = default;
    [SerializeField] private Sprite frame = null;

    public string DisplayName { get => displayName; set => displayName = value; }
    public Color Colour { get => colour; set => colour = value; }
    public Sprite Frame { get => frame; set => frame = value; }
}
