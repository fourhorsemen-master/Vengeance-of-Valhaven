using UnityEngine;

public class PoisonStabVisual : StaticAbilityObject
{
    public override float StickTime => 5f;

    public static void Create(PoisonStabVisual prefab, Vector3 position, Quaternion rotation)
    {
        Instantiate(prefab, position, rotation);
    }
}
