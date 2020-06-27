using System;
using UnityEngine;

public class HookObject : ProjectileObject
{
    public static void Fire(Actor caster, Action<GameObject> collisionCallback, Action<bool> MissCallback, float speed, Vector3 position, Quaternion rotation, float maxRange)
    {
        float stickTime = maxRange / speed;

        HookObject prefab = AbilityObjectPrefabLookup.Instance.HookObjectPrefab;
        Instantiate(prefab, position, rotation)
            .InitialiseProjectile(caster, collisionCallback, speed)
            .DestroyAfterTime(stickTime, MissCallback);
    }
}
