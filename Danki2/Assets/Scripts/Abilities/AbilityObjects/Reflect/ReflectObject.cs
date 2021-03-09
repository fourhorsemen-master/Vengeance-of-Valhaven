using UnityEngine;

public class ReflectObject : StaticAbilityObject
{
    public override float StickTime => 5f;

    [SerializeField] private ReflectVisual reflectVisualPrefab;

    public static ReflectObject Create(Transform transform, float height, Subject onReflect)
    {
        ReflectObject reflectObject = Instantiate(AbilityObjectPrefabLookup.Instance.ReflectObjectPrefab, transform);
        Vector3 position = reflectObject.transform.position;
        position.y += height;
        reflectObject.transform.position = position;
        onReflect.Subscribe(reflectObject.HandleReflect);
        return reflectObject;
    }

    private void HandleReflect() => ReflectVisual.Create(reflectVisualPrefab, transform);
}
