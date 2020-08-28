using System;
using UnityEngine;

public class SwordThrowObject : ProjectileObject
{
    public AudioSource collisionSound = null;

    public static void Fire(Actor caster, Action<GameObject> collisionCallback, float speed, Vector3 position, Quaternion rotation)
    {
        SwordThrowObject prefab = AbilityObjectPrefabLookup.Instance.SwordThrowObjectPrefab;
        Instantiate(prefab, position, rotation)
            .InitialiseProjectile(caster, collisionCallback, speed)
            .SetSticky(2f);
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if(other.gameObject != caster.gameObject) collisionSound.Play();
    }
}
