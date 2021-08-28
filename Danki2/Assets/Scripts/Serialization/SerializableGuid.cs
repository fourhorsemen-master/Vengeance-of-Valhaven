using System;
using UnityEngine;

[Serializable]
public class SerializableGuid
{
    [SerializeField] private string value;

    public static SerializableGuid NewGuid()
    {
        return new SerializableGuid { value = Guid.NewGuid().ToString() };
    }

    public override string ToString()
    {
        return value;
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        return obj.GetType() == GetType() && Equals((SerializableGuid) obj);
    }

    public override int GetHashCode()
    {
        return value != null ? value.GetHashCode() : 0;
    }

    protected bool Equals(SerializableGuid other)
    {
        return value == other.value;
    }
}
