using UnityEngine;

public class PounceObject : StaticAbilityObject
{
    public override float StickTime => 5f;

    public static void Create(Vector3 position, Quaternion rotation)
    {
        PounceObject prefab = AbilityObjectPrefabLookup.Instance.PounceObjectPrefab;
        Instantiate(prefab, position, rotation);
    }
}

