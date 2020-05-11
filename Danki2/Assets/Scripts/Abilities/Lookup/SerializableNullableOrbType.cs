using System;
using UnityEngine;

[Serializable]
public class SerializableNullableOrbType
{
    [SerializeField]
    private bool hasValue = false;
    [SerializeField]
    private OrbType value = default;

    public bool HasValue { get => hasValue; set => hasValue = value; }
    public OrbType Value { get => value; set => this.value = value; }
}
