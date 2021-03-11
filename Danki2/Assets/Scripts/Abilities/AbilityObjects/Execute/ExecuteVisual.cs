using UnityEngine;

public class ExecuteVisual : AbilityObject
{
    public static void Create(ExecuteVisual prefab, Transform transform)
    {
        Instantiate(prefab, transform);
    }
}
