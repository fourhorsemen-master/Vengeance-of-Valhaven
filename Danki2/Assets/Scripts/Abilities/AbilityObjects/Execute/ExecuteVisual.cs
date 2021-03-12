using UnityEngine;

public class ExecuteVisual : StaticAbilityObject
{
    public override float StickTime => 5f;

    public static void Create(ExecuteVisual prefab, Transform transform)
    {
        Instantiate(prefab, transform);
    }
}
