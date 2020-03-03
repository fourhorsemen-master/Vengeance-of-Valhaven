using System;
using UnityEngine;

public class FireballObject : ProjectileObject
{
    public static void Fire(Actor caster, Action<GameObject> collisionCallback, float speed, Vector3 position, Quaternion rotation)
    {
        FireballObject prefab = AbilityObjectPrefabLookup.Instance.FireballObjectPrefab;
        Instantiate(prefab, position, rotation)
            .InitialiseProjectile(caster, collisionCallback, speed);
    }
}
