using UnityEngine;

public class ReflectObject : StaticAbilityObject
{
    public override float StickTime => 5f;

    [SerializeField]
    private ReflectVisual reflectVisualPrefab = null;

    private float visualPositionOffset;

    public static ReflectObject Create(Transform transform, float height, Subject onReflect, float visualPositionOffset)
    {
        ReflectObject reflectObject = Instantiate(AbilityObjectPrefabLookup.Instance.ReflectObjectPrefab, transform);

        Vector3 position = reflectObject.transform.position;
        position.y += height;
        reflectObject.transform.position = position;
        onReflect.Subscribe(reflectObject.HandleReflect);

        reflectObject.visualPositionOffset = visualPositionOffset;
        
        return reflectObject;
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    private void HandleReflect()
    {
        ReflectVisual reflectVisual = ReflectVisual.Create(reflectVisualPrefab, transform);
        reflectVisual.transform.position += transform.forward * visualPositionOffset;
    }
}
