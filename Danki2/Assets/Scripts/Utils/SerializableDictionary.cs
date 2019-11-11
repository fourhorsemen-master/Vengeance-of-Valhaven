using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
    [SerializeField]
    private List<TKey> _keys = new List<TKey>();

    [SerializeField]
    private List<TValue> _values = new List<TValue>();

    public void OnBeforeSerialize()
    {
        _keys.Clear();
        _values.Clear();
        foreach (KeyValuePair<TKey, TValue> keyValuePair in this)
        {
            _keys.Add(keyValuePair.Key);
            _values.Add(keyValuePair.Value);
        }
    }

    public void OnAfterDeserialize()
    {
        Clear();
        for (int i = 0; i < _keys.Count; i++)
        {
            Add(_keys[i], _values[i]);
        }
    }
}
