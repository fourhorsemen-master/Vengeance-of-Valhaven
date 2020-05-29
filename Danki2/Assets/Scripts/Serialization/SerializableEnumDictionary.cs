using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Maps enum keys to values in a way that the editor can read and write.
/// </summary>
/// <typeparam name="TEnumKey">
///     We can only add new values to the end of the enum when it part of a SerializableEnumDictionary.
///     Make sure this is stated in a comment on the enum.
/// </typeparam>
/// <typeparam name="TValue"></typeparam>
[Serializable]
public class SerializableEnumDictionary<TEnumKey, TValue> : ISerializationCallbackReceiver where TEnumKey : Enum
{
    [SerializeField]
    private List<string> _keys = new List<string>();

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
        get => _dictionary[key];
        set => _dictionary[key] = value;
    }

    public void OnBeforeSerialize()
    {
        _keys.Clear();
        _values.Clear();
        foreach (KeyValuePair<TEnumKey, TValue> keyValuePair in _dictionary)
        {
            _keys.Add(keyValuePair.Key.ToString());
            _values.Add(keyValuePair.Value);
        }
    }

    public void OnAfterDeserialize()
    {
        for (int i = 0; i < _keys.Count; i++)
        {
            TEnumKey enumKey = (TEnumKey) Enum.Parse(typeof(TEnumKey), _keys[i]);
            _dictionary[enumKey] = _values[i];
        }
    }
}