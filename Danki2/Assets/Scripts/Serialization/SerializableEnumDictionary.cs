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

    public Dictionary<TEnumKey, TValue>.KeyCollection Keys => _dictionary.Keys;

    public Dictionary<TEnumKey, TValue>.ValueCollection Values => _dictionary.Values;

    public SerializableEnumDictionary(TValue defaultValue)
    {
        EnumUtils.ForEach<TEnumKey>(key => _dictionary.Add(key, defaultValue));
    }

    public SerializableEnumDictionary(Func<TValue> defaultValueProvider)
    {
        EnumUtils.ForEach<TEnumKey>(key => _dictionary.Add(key, defaultValueProvider()));
    }

    public TValue this[TEnumKey key]
    {
        get => _dictionary[key];
        set => _dictionary[key] = value;
    }

    public void Add(TEnumKey key, TValue value)
    {
        _dictionary.Add(key, value);
    }

    public bool Remove(TEnumKey key)
    {
        return _dictionary.Remove(key);
    }

    public bool ContainsKey(TEnumKey key)
    {
        return _dictionary.ContainsKey(key);
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
            if (!Enum.IsDefined(typeof(TEnumKey), _keys[i])) continue;
            TEnumKey enumKey = EnumUtils.FromString<TEnumKey>(_keys[i]);
            _dictionary[enumKey] = _values[i];
        }
    }
}
