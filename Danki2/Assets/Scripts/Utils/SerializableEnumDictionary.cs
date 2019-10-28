using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SerializableEnumDictionary<TEnumKey, TValue> : ISerializationCallbackReceiver where TEnumKey : Enum
{
    [SerializeField]
    private List<TEnumKey> keys = new List<TEnumKey>();

    [SerializeField]
    private List<TValue> values = new List<TValue>();

    private Dictionary<TEnumKey, TValue> dictionary = new Dictionary<TEnumKey, TValue>();

    public SerializableEnumDictionary(TValue defaultValue)
    {
        foreach (TEnumKey key in Enum.GetValues(typeof(TEnumKey)))
        {
            dictionary.Add(key, defaultValue);
        }
    }

    public SerializableEnumDictionary(SerializableEnumDictionary<TEnumKey, TValue> dictionary)
    {
        foreach (TEnumKey key in Enum.GetValues(typeof(TEnumKey)))
        {
            this.dictionary.Add(key, dictionary[key]);
        }
    }

    public TValue this[TEnumKey key]
    {
        get { return dictionary[key]; }
        set { dictionary[key] = value; }
    }

    public void OnBeforeSerialize()
    {
        keys.Clear();
        values.Clear();
        foreach (KeyValuePair<TEnumKey, TValue> keyValuePair in dictionary)
        {
            keys.Add(keyValuePair.Key);
            values.Add(keyValuePair.Value);
        }
    }

    public void OnAfterDeserialize()
    {
        for (int i = 0; i < keys.Count; i++)
        {
            dictionary[keys[i]] = values[i];
        }
    }
}