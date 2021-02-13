using UnityEngine;

public class BiteObject : StaticAbilityObject
{
    public override float StickTime => 5f;

    public static void Create(Vector3 position, Quaternion rotation)
    {
        BiteObject prefab = AbilityObjectPrefabLookup.Instance.BiteObjectPrefab;
        Instantiate(prefab, position, rotation);
    }
}
