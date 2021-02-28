using UnityEngine;

public class CleaveObject : StaticAbilityObject
{
    public override float StickTime => 5f;

    public static CleaveObject Create(Vector3 position, Quaternion rotation)
    {
        CleaveObject prefab = AbilityObjectPrefabLookup.Instance.CleaveObjectPrefab;
        return Instantiate(prefab, position, rotation);
    }
}
