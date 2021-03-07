using UnityEngine;

public class BackstabObject : StaticAbilityObject
{
    public override float StickTime => 5f;

    public static BackstabObject Create(Vector3 position, Quaternion rotation)
    {
        BackstabObject prefab = AbilityObjectPrefabLookup.Instance.BackstabObjectPrefab;
        return Instantiate(prefab, position, rotation);
    }
}
