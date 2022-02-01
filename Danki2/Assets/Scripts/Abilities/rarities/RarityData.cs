using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RarityData
{
    [SerializeField] private string displayName = "";
    [SerializeField] private int weighting = 0;
    [SerializeField] private Color colour = default;
    [SerializeField] private Sprite frame = null;
    [SerializeField] private List<Augmentation> augmentations = null;

    public string DisplayName { get => displayName; set => displayName = value; }
    public int Weighting { get => weighting; set => weighting = value; }
    public Color Colour { get => colour; set => colour = value; }
    public Sprite Frame { get => frame; set => frame = value; }
    public List<Augmentation> Augmentations { get => augmentations; set => augmentations = value; }
}
