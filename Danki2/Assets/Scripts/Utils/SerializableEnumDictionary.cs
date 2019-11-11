using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SerializableEnumDictionary<TEnumKey, TValue> : ISerializationCallbackReceiver where TEnumKey : Enum
{
    [SerializeField]
    private List<TEnumKey> _keys = new List<TEnumKey>();

    [SerializeField]
    private List<TValue> _values = new List<TValue>();

    private Dictionary<TEnumKey, TValue> _dictionary = new Dictionary<TEnumKey, TValue>();

    public SerializableEnumDictionary(TValue defaultValue)
    {
        foreach (TEnumKey key in Enum.GetValues(typeof(TEnumKey)))
        {
            _dictionary.Add(key, defaultValue);
        }
    }

    public SerializableEnumDictionary(SerializableEnumDictionary<TEnumKey, TValue> dictionary)
    {
        foreach (TEnumKey key in Enum.GetValues(typeof(TEnumKey)))
        {
            _dictionary.Add(key, dictionary[key]);
        }
    }

    public TValue this[TEnumKey key]
    {
        get { return _dictionary[key]; }
        set { _dictionary[key] = value; }
    }

    public void OnBeforeSerialize()
    {
        _keys.Clear();
        _values.Clear();
        foreach (KeyValuePair<TEnumKey, TValue> keyValuePair in _dictionary)
        {
            _keys.Add(keyValuePair.Key);
            _values.Add(keyValuePair.Value);
        }
    }

    public void OnAfterDeserialize()
    {
        for (int i = 0; i < _keys.Count; i++)
        {
            _dictionary[_keys[i]] = _values[i];
        }
    }
}