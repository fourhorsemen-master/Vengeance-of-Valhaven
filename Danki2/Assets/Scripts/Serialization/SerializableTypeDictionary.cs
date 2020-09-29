using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SerializableTypeDictionary<TValue> : Dictionary<Type, TValue>, ISerializationCallbackReceiver
{
    [SerializeField]
    private List<string> keys = new List<string>();

    [SerializeField]
    private List<TValue> values = new List<TValue>();

    public void OnBeforeSerialize()
    {
        keys.Clear();
        values.Clear();
        foreach (KeyValuePair<Type,TValue> keyValuePair in this)
        {
            keys.Add(keyValuePair.Key.AssemblyQualifiedName);
            values.Add(keyValuePair.Value);
        }
    }

    public void OnAfterDeserialize()
    {
        Clear();
        for (int i = 0; i < keys.Count; i++)
        {
            Type type = Type.GetType(keys[i]);
            if (type != null) Add(type, values[i]);
        }
    }
}
