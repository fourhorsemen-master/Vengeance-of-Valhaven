using UnityEngine;

public class RendObject : StaticAbilityObject
{    
    public override float StickTime => 2f;

    public static RendObject Create(Transform parentTransform, Vector3 position)
    {
        RendObject rendObject = Instantiate(AbilityObjectPrefabLookup.Instance.RendObjectPrefab, position, Quaternion.LookRotation(Vector3.right), parentTransform);

        return rendObject;
    }
}
