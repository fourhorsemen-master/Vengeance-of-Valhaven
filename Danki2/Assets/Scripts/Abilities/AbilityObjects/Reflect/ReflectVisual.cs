using UnityEngine;

public class ReflectVisual : StaticAbilityObject
{
    public override float StickTime => 5;

    public static ReflectVisual Create(ReflectVisual prefab, Transform transform)
    {
        return Instantiate(prefab, transform.position, transform.rotation);
    }
}
