using UnityEngine;

public class ExecuteObject : StaticAbilityObject
{
    public override float StickTime => 5f;

    public static void Create(Vector3 position, float scaleFactor = 1f)
    {
        ExecuteObject prefab = AbilityObjectPrefabLookup.Instance.ExecuteObjectPrefab;
        ExecuteObject smashObject = Instantiate(prefab, position, Quaternion.identity);
        smashObject.gameObject.transform.localScale *= scaleFactor;
    }
}
