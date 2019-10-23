using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
    [SerializeField]
    private readonly List<TKey> keys = new List<TKey>();

    [SerializeField]
    private readonly List<TValue> values = new List<TValue>();

    public void OnBeforeSerialize()
    {
        keys.Clear();
        values.Clear();
        foreach (KeyValuePair<TKey, TValue> keyValuePair in this)
        {
            keys.Add(keyValuePair.Key);
            values.Add(keyValuePair.Value);
        }
    }

    public void OnAfterDeserialize()
    {
        Clear();
        for (int i = 0; i < keys.Count; i++)
        {
            Add(keys[i], values[i]);
        }
    }
}
