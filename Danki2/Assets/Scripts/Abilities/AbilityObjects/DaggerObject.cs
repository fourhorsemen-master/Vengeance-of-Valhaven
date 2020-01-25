using System;
using UnityEngine;

public class DaggerObject : ProjectileObject
{
    public static void Fire(Actor caster, Action<GameObject> collisionCallback, float speed, Vector3 position, Quaternion rotation)
    {
        var prefab = AbilityObjectPrefabLookup.Instance.DaggerObjectPrefab;
        Instantiate(prefab, position, rotation)
            .InitialiseProjectile(caster, collisionCallback, speed);
    }
}
