using UnityEngine;

public class ReflectVisual : StaticAbilityObject
{
    public override float StickTime => 5;

    public static void Create(ReflectVisual prefab, Transform transform)
    {
        Instantiate(prefab, transform.position, transform.rotation);
    }
}
