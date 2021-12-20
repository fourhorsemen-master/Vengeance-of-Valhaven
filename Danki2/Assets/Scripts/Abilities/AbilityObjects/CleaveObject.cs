using UnityEngine;

public class CleaveObject : AbilityObject
{
    public static CleaveObject Create(Vector3 position, Quaternion rotation)
    {
        CleaveObject prefab = AbilityObjectPrefabLookup.Instance.CleaveObjectPrefab;
        return Instantiate(prefab, position, rotation);
    }
}
