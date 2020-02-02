using System;
using UnityEngine;

public class DaggerObject : ProjectileObject
{
    public static void Fire(Actor caster, Action<GameObject> collisionCallback, float speed, Vector3 position, Quaternion rotation)
    {
        var prefab = AbilityObjectPrefabLookup.Instance.DaggerObjectPrefab;
        Instantiate(prefab, position, rotation)
            .InitialiseProjectile(caster, collisionCallback, speed)
            .SetSticky(5f);
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (GameObject.ReferenceEquals(_caster.gameObject, other.gameObject)) return;

        base.OnTriggerEnter(other);

        TrailRenderer tr = gameObject.GetComponentInChildren<TrailRenderer>();
        Destroy(tr);
    }
}
