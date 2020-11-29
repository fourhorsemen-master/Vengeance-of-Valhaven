using System;
using UnityEngine;

[Serializable]
public abstract class SerializableEffectData
{
    [SerializeField] private string displayName = "";
    [SerializeField] private Sprite sprite = null;

    public string DisplayName { get => displayName; set => displayName = value; }
    public Sprite Sprite { get => sprite; set => sprite = value; }
}

[Serializable]
public class SerializableActiveEffectData : SerializableEffectData {}

[Serializable]
public class SerializablePassiveEffectData : SerializableEffectData {}

[Serializable]
public class SerializableStackingEffectData : SerializableEffectData
{
    [SerializeField] private int maxStackSize = 0;
    [SerializeField] private float duration = 0;

    public int MaxStackSize { get => maxStackSize; set => maxStackSize = value; }
    public float Duration { get => duration; set => duration = value; }
}
