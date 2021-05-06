using UnityEngine;

public class GuidedOrbExplosion : StaticAbilityObject
{
    public override float StickTime => 5f;

    public static void Create(GuidedOrbExplosion prefab, Vector3 position)
    {
        Instantiate(prefab, position, Quaternion.identity);
    }
}
