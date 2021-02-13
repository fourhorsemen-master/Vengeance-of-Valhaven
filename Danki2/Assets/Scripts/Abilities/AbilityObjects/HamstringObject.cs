using UnityEngine;

public class HamstringObject : StaticAbilityObject
{
    public override float StickTime => 5f;

    public static HamstringObject Create(Vector3 position, Quaternion rotation)
    {
        return Instantiate(AbilityObjectPrefabLookup.Instance.HamstringObjectPrefab, position, rotation);
    }
}
