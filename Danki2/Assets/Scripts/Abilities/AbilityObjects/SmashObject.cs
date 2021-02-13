using UnityEngine;

public class SmashObject : StaticAbilityObject
{
    public override float StickTime => 5f;

    public static void Create(Vector3 position, float scaleFactor = 1f)
    {
        SmashObject prefab = AbilityObjectPrefabLookup.Instance.SmashObjectPrefab;
        SmashObject smashObject = Instantiate(prefab, position, Quaternion.identity);
        smashObject.gameObject.transform.localScale *= scaleFactor;
    }
}
