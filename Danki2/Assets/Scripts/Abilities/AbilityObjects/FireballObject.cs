using System;
using UnityEngine;

public class FireballObject : ProjectileObject
{
    public static void Fire(Actor caster, Action<GameObject> collisionCallback, Vector3 position, Quaternion rotation, float speed)
    {
        var prefab = AbilityObjectPrefabLookup.Instance.FireballObjectPrefab;
        var fireballObject = Instantiate(prefab, position, rotation);
        fireballObject._caster = caster;
        fireballObject._collisionCallback = collisionCallback;
        fireballObject._speed = speed;
    }
}
