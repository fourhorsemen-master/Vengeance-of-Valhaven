using System;
using UnityEngine;

public class SwordThrowObject : ProjectileObject
{
    public AudioSource _collisionSound = null;

    public static void Fire(Actor caster, Action<GameObject> collisionCallback, float speed, Vector3 position, Quaternion rotation)
    {
        SwordThrowObject prefab = AbilityObjectPrefabLookup.Instance.SwordThrowObjectPrefab;
        Instantiate(prefab, position, rotation)
            .InitialiseProjectile(caster, collisionCallback, speed)
            .SetSticky(5f);
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        _collisionSound.Play();

        if (!ReferenceEquals(caster.gameObject, other.gameObject))
        {
            TrailRenderer tr = gameObject.GetComponentInChildren<TrailRenderer>();
            Destroy(tr);
        }
    }
}
