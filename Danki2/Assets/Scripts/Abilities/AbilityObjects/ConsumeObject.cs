using UnityEngine;

public class ConsumeObject : StaticAbilityObject
{
    public override float StickTime => 5f;

    public static void Create(Transform transform)
    {
        Instantiate(AbilityObjectPrefabLookup.Instance.ConsumeObjectPrefab, transform);
    }
}
