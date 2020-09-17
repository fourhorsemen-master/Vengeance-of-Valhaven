using UnityEngine;

public class ParryVisual : StaticAbilityObject
{
    public override float StickTime => 2;

    public static void Create(ParryVisual prefab, Vector3 position)
    {
        Instantiate(prefab, position, Quaternion.identity);
    }
}
