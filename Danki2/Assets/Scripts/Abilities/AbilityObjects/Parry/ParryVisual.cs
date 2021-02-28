using UnityEngine;

public class ParryVisual : AbilityObject
{
    public static void Create(ParryVisual prefab, Transform transform)
    {
        Instantiate(prefab, transform);
    }
}
