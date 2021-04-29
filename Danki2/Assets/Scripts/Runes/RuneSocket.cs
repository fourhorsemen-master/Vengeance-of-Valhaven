using System;
using UnityEngine;

[Serializable]
public class RuneSocket
{
    [SerializeField] private bool hasRune = false;
    [SerializeField] private Rune rune = default;

    public bool HasRune { get => hasRune; set => hasRune = value; }
    public Rune Rune { get => rune; set => rune = value; }
}
